
using Microsoft.AspNetCore.SignalR;
using shopmallService.Hubs;
using shopmallService.Interfaces;

namespace shopmallService.Services
{
    public class OrderBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private readonly IRedisQueueService _redisQueueService;
        private readonly IHubContext<OrderNotificationHub> _hubContext;
        public OrderBackgroundService(IServiceScopeFactory scopeFactory,
            ILogger<OrderBackgroundService> logger,
            IRedisQueueService redisQueueService,
            IHubContext<OrderNotificationHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _redisQueueService = redisQueueService;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("订单后台服务已启动");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
        

                    //从队列中拿出订单
                    var order = await _redisQueueService.ConsumeOrderAsync();



                    if (order != null)
                    {
                        _logger.LogInformation($"开始处理订单: {order}");

                        // 在作用域中解析Scoped服务
                        using var scope = _scopeFactory.CreateScope();
                        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                        //保存订单
                        var result = orderService.addOrders(order);
                        string key = order.PersonalId + ":order";
                        WebSocketMiddleware.SendMessageToUser(key, new
                        {
                            type = "ORDER_COMPLETED",
                            data = new
                            {
                                orderId=order.OrderId,
                                orderNo = order.OrderNo.ToString(),
                                personalId = order.PersonalId.ToString(),
                                flag=result.Flag,
                                message = result.Message

                            }
                        });
                    }
                    else
                    {
                        // 如果没有消息，短暂休眠避免过度占用CPU
                        await Task.Delay(1000, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"处理订单时发生错误: {ex.Message}");
                
                    await Task.Delay(5000, stoppingToken); // 出错后等待5秒再继续
                }
            }



    }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("订单处理后台服务正在停止");
            await base.StopAsync(cancellationToken);
        }


    }
}
