using FluentScheduler;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using shopmallService.Hubs;
using System.Collections;
namespace shopmallService.Jobs
{
    public class JobScheduler
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly JobFactory _jobFactory;
        private readonly IScheduler _scheduler;
        // 注入 Quartz 调度器
        public JobScheduler(IServiceScopeFactory scopeFactory, JobFactory jobFactory)
        {
            _scopeFactory = scopeFactory;
            _jobFactory = jobFactory;
            _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            _scheduler.Start();
 
        }

        /// <summary>
        /// 添加秒杀定时任务：到点执行 SeckillStartJob
        /// </summary>
        public async Task AddSeckillStartJob(dynamic item)
        {
            string json = item.GetRawText();
            var paramsObj = new { seckillTime = "", businessList = new ArrayList() };
            paramsObj = JsonConvert.DeserializeAnonymousType(json, paramsObj);
            //指定触发时间 
            //两个任务 1.触发时间点提前5分钟 预热商品  2.触发时间点延后5秒加载商品



            string time = paramsObj.seckillTime;
            ArrayList businessList= paramsObj.businessList;
            string jsonbusiness = JsonConvert.SerializeObject(businessList);
            DateTime startTime = DateTime.Parse($"2000-01-01 {time}"); // 秒杀开始时间


            var jobId = $"seckill_job_{time.ToString()}";

            // 1. 防止重复添加
            var jobKey = new JobKey(jobId);
            if (await _scheduler.CheckExists(jobKey))
            {
                return;
            }

            // 2. 创建 Job
            //指定自定义的JobFactory
            _scheduler.JobFactory = _jobFactory;

            var job = JobBuilder.Create<JobChat>()
                .WithIdentity(jobKey)
                .UsingJobData("seckillTime", time) // 传秒杀Time
                .UsingJobData("businessList", jsonbusiness)
                .Build();


            // 3.1 创建触发器触发时间点提前1分钟 预热商品
            DateTime autotime = startTime.AddMinutes(-1);
            int hh = ((DateTime)autotime).Hour;
            int mm = ((DateTime)autotime).Minute;

            var trigger1 = TriggerBuilder.Create()
                .WithIdentity($"trigger1_{jobId}")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hh, mm)) //每日固定时间点执行
                .ForJob(job)
                .UsingJobData("triggerflag", "hotseckill")

                .Build();


            await _scheduler.ScheduleJob(job,trigger1);

            // 3.2 触发时间点延后5秒加载商品
            autotime = startTime.AddSeconds(5);
             hh = ((DateTime)autotime).Hour;
             mm = ((DateTime)autotime).Minute;
             int ss= ((DateTime)autotime).Second;

            // 构建Cron表达式
            string cronExpression = $"{ss} {mm} {hh} * * ?";

            var trigger2 = TriggerBuilder.Create()
                .WithIdentity($"trigger2_{jobId}")
                //.WithSchedule(CronScheduleBuilder.CronSchedule(cronExpression)) //每日固定时间点执行
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hh, mm)) //每日固定时间点执行
                .ForJob(job)
                .UsingJobData("triggerflag", "sendseckill")
                .Build();

            await _scheduler.ScheduleJob(trigger2);

        }
    }
}
