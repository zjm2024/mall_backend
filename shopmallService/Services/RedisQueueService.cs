using NetTaste;
using Newtonsoft.Json;
using publicClassLibrary.Entitys;
using shopmallService.Interfaces;
using StackExchange.Redis;

namespace shopmallService.Services
{
    public class RedisQueueService : IRedisQueueService, IDisposable
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly string _queueName = "orders_queue";
        public RedisQueueService(IConnectionMultiplexer redis)
        {
            // 连接到Redis服务器
            _redis = redis;
            _database = redis.GetDatabase();



        }
        public async Task<bool> PublishOrderAsync(Orders order)
        {
            // 将订单序列化为JSON字符串
            try
            {
                var message = JsonConvert.SerializeObject(order);

                // 使用LPUSH将消息添加到列表的头部
                long result = await _database.ListLeftPushAsync(_queueName, message);
                return result > 0;
            }
            catch (Exception ex)
            {
                // 处理异常情况 写到日志里
                return false;
                //Console.WriteLine($"插入失败: {ex.Message}");
            }
        }

        public async Task<Orders> ConsumeOrderAsync()
        {
            // 使用BRPOP阻塞式地从列表尾部取出消息，超时时间为5秒
            var result = await _database.ListRightPopAsync(_queueName);

            if (result.HasValue)
            {
                // 反序列化消息为订单对象
                var message = result.ToString();
                return JsonConvert.DeserializeObject<Orders>(message);
            }

            return null;
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }

    }
}
