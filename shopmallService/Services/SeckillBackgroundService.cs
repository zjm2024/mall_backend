using Microsoft.AspNetCore.DataProtection.KeyManagement;
using publicClassLibrary.Interfaces;
using shopmallService.Interfaces;
using StackExchange.Redis;
using System.Diagnostics;


namespace shopmallService.Services
{
    public class SeckillBackgroundService: BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private readonly IRedisQueueService _redisQueueService;
 
        public SeckillBackgroundService(IServiceScopeFactory scopeFactory,
            ILogger<SeckillBackgroundService> logger,
            IRedisQueueService redisQueueService)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _redisQueueService = redisQueueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            _logger.LogInformation("秒杀活动监控服务已启动");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                  
                    // 在作用域中解析Scoped服务
                    using var scope = _scopeFactory.CreateScope();
                    var seckillService = scope.ServiceProvider.GetRequiredService<ISeckillService>();

                    // 获取所有秒杀活动ID
                    var activityIds = await _redisQueueService.GetAllSeckillActivityIdsAsync();
                    foreach (var activityId in activityIds)
                    {
                        var seckill = await _redisQueueService.GetSeckillActivityAsync(activityId);
                        if (seckill == null) continue;

                        var now = DateTime.Now;
                        // 1. 已发布但未开始 → 到开始时间 → 改为进行中
                        if (seckill.Status == 0 && now >= seckill.StartTime && now < seckill.EndTime)
                        {
                            seckill.Status = 1;//进行中
                            await _redisQueueService.UpdateSeckillStatusAsync(activityId, seckill.Status);
                            // 更新数据库
                            var updateColums = new string[] { "status", "updateTime" };

                            seckillService.updateSeckillStatus(seckill, updateColums);


                            // 通知前端：活动开始
                            var message = "秒杀活动开始";
                            WebSocketMiddleware.SendMessageToAll( new
                            {
                                type = "SECKILL_STATUS",
                                data = new
                                {
                                    seckillId = seckill.SeckillId,
                                    result=seckill,
                                    flag = 0,
                                    message = message

                                }
                            });
                 


                            _logger.LogInformation($"秒杀活动 {activityId} 已开始");
                        }
                        /*
                        // 2. 进行中 → 到结束时间 → 改为已结束
                        if (activity.Status == SeckillStatus.Ongoing && now >= activity.EndTime)
                        {
                            activity.Status = SeckillStatus.Ended;
                            await redisHelper.UpdateSeckillStatusAsync(activityId, SeckillStatus.Ended);
                            // 更新数据库
                            var dbActivity = await dbContext.SeckillActivities.FindAsync(activityId);
                            if (dbActivity != null)
                            {
                                dbActivity.Status = SeckillStatus.Ended;
                                await dbContext.SaveChangesAsync();
                            }
                            // 通知前端：活动结束
                            var message = new WebSocketMessage
                            {
                                Type = "ActivityEnded",
                                ActivityId = activityId,
                                Data = new { Status = "Ended", Message = "秒杀活动已结束" }
                            };
                            await _webSocketManager.SendMessageToAllAsync(JsonSerializer.Serialize(message));
                            _logger.LogInformation($"秒杀活动 {activityId} 已结束");
                        }
                        */
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "秒杀活动监控服务执行异常");
                }

                // 等待检查间隔
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("秒杀活动监控服务已停止");
     
            
            }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("秒杀活动监控服务正在停止");
            await base.StopAsync(cancellationToken);
        }

    }
}
