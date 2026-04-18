using FluentScheduler;
using shopmallService.Interfaces;
using System;

namespace shopmallService.Jobs
{
    public class SeckillTimesJob : IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SeckillTimesJob> _logger;

        public SeckillTimesJob(IServiceScopeFactory scopeFactory, ILogger<SeckillTimesJob> logger)
        {
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
                _logger.LogInformation("SeckillTimesJob 定时任务 开始读取 Redis 去重秒杀时间点 保存到 Redis");

                using var scope = _scopeFactory.CreateScope();
                var seckillService = scope.ServiceProvider.GetRequiredService<ISeckillService>();



                var result = await seckillService.saveAllSeckillTimesAsync();
               
                if (result==false)
                {
                    _logger.LogWarning("Redis 中未保存到秒杀时间点配置");
                    return;
                }

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【定时任务异常】读取 Redis 秒杀时间失败");
            }
        }
    }
}