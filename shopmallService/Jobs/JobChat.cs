using Newtonsoft.Json;
using publicClassLibrary.Entitys;
using Quartz;
using shopmallService.Interfaces;
using shopmallService.Services;
using System.Collections;


namespace shopmallService.Jobs
{
    public class JobChat : IJob
    {
        // private readonly IHubContext<ChatHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<JobChat> _logger;
        public JobChat(IServiceScopeFactory scopeFactory,ILogger<JobChat> logger)  // IHubContext<ChatHub> hubContext
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            // _hubContext = hubContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //执行完删除当前任务
          //  IScheduler scheduler = context.Scheduler;
           // JobKey jobkey = context.JobDetail.Key;

          //  scheduler.DeleteJob(jobkey);

            //执行定时任务
            _logger.LogInformation("Tasking executed at {Time}", DateTime.Now.ToString());

            using var scope = _scopeFactory.CreateScope();
            var seckillService = scope.ServiceProvider.GetRequiredService<ISeckillService>();

            var jobDataMap = context.MergedJobDataMap;
            var triggerflag = jobDataMap.GetString("triggerflag");
            var seckillTime = jobDataMap.GetString("seckillTime");
            var json = jobDataMap.GetString("businessList");
            List<dynamic> list = System.Text.Json.JsonSerializer.Deserialize<List<dynamic>>(json);

            if (triggerflag == "hotseckill")
            {
                //预热秒杀活动到redis
                foreach (var item in list)
                {
                    var paramsObj = new { businessId = 0 };
                    string entityJson = item.GetRawText();
                    dynamic entity = JsonConvert.DeserializeAnonymousType(entityJson, paramsObj);
                    var businessId= entity.businessId;
                    var activedate = DateTime.Today.ToString("yyyy-MM-dd") ;

                    var activetime =DateTime.Parse($"{activedate} {seckillTime}");

                    await seckillService.saveHotSeckillProductsAsync(businessId, activetime);
                }

       
            }


            if (triggerflag == "sendseckill")
            {
                //发送消息给所有客户端
                // 通知前端：活动开始d
                var message = jobDataMap.GetString("message");
                var seckilltime = jobDataMap.GetString("seckilltime");
                //读取当前预热的秒杀活动
               await seckillService.getHotSeckillProductsAsync()
                WebSocketMiddleware.SendMessageToAll(new
                {
                    type = "SECKILL_TIMES",
                    data = new
                    {

                        result = seckilltime,
                        flag = 0,
                        message = message

                    }
                });




            }
        }
    }
}


