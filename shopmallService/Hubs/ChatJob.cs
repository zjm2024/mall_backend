using Microsoft.AspNetCore.SignalR;
using Quartz;
using shopmallService.Services;
using System.Security.Principal;

namespace shopmallService.Hubs
{
    public class ChatJob : IJob
    {
        // private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<ChatJob> _logger;
        public ChatJob(ILogger<ChatJob> logger)  // IHubContext<ChatHub> hubContext
        {
            _logger = logger;
            // _hubContext = hubContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //执行完删除当前任务
            IScheduler scheduler = context.Scheduler;
            JobKey jobkey = context.JobDetail.Key;

            scheduler.DeleteJob(jobkey);

            //执行定时任务
            _logger.LogInformation("Tasking executed at {Time}", DateTime.Now.ToString());


            var jobDataMap = context.MergedJobDataMap;
            var fixedflag = jobDataMap.GetString("fixedflag");


            if (fixedflag == "seckilltimers")
            {
                //发送消息给所有客户端
                // 通知前端：活动开始
                var message = jobDataMap.GetString("message");
                var seckilltime = jobDataMap.GetString("seckilltime");
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


