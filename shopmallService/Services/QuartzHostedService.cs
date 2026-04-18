using Microsoft.VisualBasic;
using Newtonsoft.Json;
using publicClassLibrary.Consts;
using publicClassLibrary.Entitys;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Services;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using shopmallService.Hubs;
using shopmallService.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace shopmallService.Services
{
    public class QuartzHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private readonly IRedisQueueService _redisQueueService;
        private readonly ChatJobFactory _jobFactory;
        private readonly IScheduler _scheduler;
        private string _dateformat = "yyyy-MM-dd HH:mm";
        public QuartzHostedService(IServiceScopeFactory scopeFactory, ILogger<QuartzHostedService> logger, IRedisQueueService redisQueueService, ChatJobFactory jobFactory)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _redisQueueService= redisQueueService;
            _jobFactory = jobFactory;
            _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            _scheduler.Start();
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
    

            var autoHintInterval = "10000";

   
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                    // 在作用域中解析Scoped服务
                    using var scope = _scopeFactory.CreateScope();
                    var datadictService = scope.ServiceProvider.GetRequiredService<IDataDictService>();

        

                    var nowstring = DateTime.Now.ToString(_dateformat);
                    DateTime now = DateTime.ParseExact(nowstring, _dateformat, null);

                    //查询当前有秒杀活动的商城


                    //查询单据 8:00,8:30 各个时间点 要形成一个 触发任务清单
                    //先读缓存。如果缓存没有则读数据库
                     var objout=  await datadictService.getDataDictByCode(CacheConst.KeySeckillTimes);


                    if (objout.Flag == 1 && objout.Result != null)
                    {
                        string json = objout.Result.ToString();
                        List<string> list = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);

          
        
                        foreach (var time in list)
                        {
                            var taskid = "task_begin:" + time;
                            var fixedflag = "seckilltimers";
                            var seckilltime = time;
                            var message = time + "秒杀时间段开始";
                             
                            DateTime autotime = DateTime.Parse($"2000-01-01 {time}");
                            int hh = ((DateTime)autotime).Hour;
                            int mm = ((DateTime)autotime).Minute;


                            var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals("group"));


                            var jobKey = new JobKey(taskid, "group"); // 替换为你的作业名和组名
                            var jobExists = await _scheduler.CheckExists(jobKey);



                            if (!jobExists)
                            {
                                //指定触发时间 触发时间在实际提醒时间后延迟5秒
                                DateTime triggerTime = autotime.AddSeconds(5);
                                //指定自定义的JobFactory
                                _scheduler.JobFactory = _jobFactory;

                                //创建Job
                                var sendMsgJob = JobBuilder.Create<ChatJob>()
                                    .WithIdentity(taskid, "group")
                                    .UsingJobData("fixedflag", fixedflag)
                                    .UsingJobData("seckilltime", seckilltime)
                                    .UsingJobData("message", message)

                                    .Build();
                                //创建触发器
                                var sendMsgTrigger = TriggerBuilder.Create()
                                    .WithIdentity("trigger-" + taskid, "trigger-group-group")
                                    //.StartNow()
                                    //.StartAt(triggerTime)
                                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hh, mm)) //每日的09:30执行

                                    .ForJob(sendMsgJob)
                                    .Build();


                                //把Job和触发器放入调度器中
                                await _scheduler.ScheduleJob(sendMsgJob, sendMsgTrigger);

                                _logger.LogInformation("服务启动执行时间为:" + hh.ToString() + '-' + mm.ToString());


                            }
                        }


                    }


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "定时任务服务执行异常");
                }


                await Task.Delay(Convert.ToInt32(autoHintInterval), stoppingToken); // 每秒检查一次，可以根据需要调整间隔时间
            }

            _logger.LogInformation("定时任务服务已停止");


        }
  
    }
}
