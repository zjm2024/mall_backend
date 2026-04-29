using FluentScheduler;
using Newtonsoft.Json;
using publicClassLibrary.Consts;
using publicClassLibrary.Entitys;
using shopmallService.Hubs;
using shopmallService.Interfaces;
using System.Collections;
using System.Data;
using System.Text.Json;

namespace shopmallService.Jobs
{
    public class SeckillTimesJob : IJob
    {
        private readonly JobScheduler _jobScheduler;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SeckillTimesJob> _logger;



        public SeckillTimesJob(JobScheduler jobScheduler, IServiceScopeFactory scopeFactory, ILogger<SeckillTimesJob> logger)
        {
            _jobScheduler = jobScheduler;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        /// <summary>
        /// 定时执行的核心方法
        /// </summary>
        public async void Execute()
        {
            try
            {
                _logger.LogInformation("SeckillTimesJob 定时任务 启动秒杀时间点触发器");

                // 在作用域中解析Scoped服务
                using var scope = _scopeFactory.CreateScope();
                var seckillService = scope.ServiceProvider.GetRequiredService<ISeckillService>();
                var datadictService = scope.ServiceProvider.GetRequiredService<IDataDictService>();


                //查询单据 8:00,8:30 各个时间点 要形成一个 触发任务清单
                //先读缓存。如果缓存没有则读数据库
                var objout = await datadictService.getDataDictByCode(CacheConst.KeySeckillTimes);
                if (objout.Flag == 1 && objout.Result != null)
                {
                    string json = objout.Result.ToString();
                    List<dynamic> list = System.Text.Json.JsonSerializer.Deserialize<List<dynamic>>(json);


                    //测试数据  begin
                    var ddd = new ArrayList();
                    ddd.Add(new { BusinessId = 107 });
                    ddd.Add(new { BusinessId = 358 });

                    var aaa = new { seckillTime = "11:50", businessList = ddd };
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(aaa);
                    using JsonDocument doc = JsonDocument.Parse(jsonString);
                    JsonElement jsonElement = doc.RootElement;

                    list.Clear();

                    list.Add(jsonElement);

                    //测试数据 end

                    foreach (var item in list)
                    {
                        await _jobScheduler.AddSeckillStartJob(item);
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动秒杀时间点触发器失败");
            }
        }
    }
}