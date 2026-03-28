using publicClassLibrary.Entitys;
using publicClassLibrary.Interfaces;
using StackExchange.Redis;

namespace publicClassLibrary.Services
{
    public class RedisQueueService : IRedisQueueService, IDisposable
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
   
        public RedisQueueService(IConnectionMultiplexer redis)
        {
            // 连接到Redis服务器
            _redis = redis;
            _db = redis.GetDatabase();
        }


        
        public async Task<string?> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _db.StringSetAsync(key, value);
  
        }


        public async Task<bool> DelKeyAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }




        public async Task<bool> PublishOrderAsync(Orders order)
        {
            // 将订单序列化为JSON字符串
            try
            {
                 string _queueName = "orders_queue";
                 var message = Newtonsoft.Json.JsonConvert.SerializeObject(order);

                // 使用LPUSH将消息添加到列表的头部
                long result = await _db.ListLeftPushAsync(_queueName, message);
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
            string _queueName = "orders_queue";
            var result = await _db.ListRightPopAsync(_queueName);

            if (result.HasValue)
            {
                // 反序列化消息为订单对象
                var message = result.ToString();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Orders>(message);
            }

            return null;
        }




        // 缓存秒杀活动
        public async Task SetSeckillActivityAsync(SeckillActivities seckill)
        {
            string key = $"Seckill:Activity:{seckill.SeckillId}";
            await _db.StringSetAsync(key, Newtonsoft.Json.JsonConvert.SerializeObject(seckill), TimeSpan.FromDays(7));
            // 将活动ID加入监控列表
            await _db.SortedSetAddAsync("Seckill:Activities:All", seckill.SeckillId.ToString(), seckill.StartTime.Ticks);
        }

        // 获取秒杀活动
        public async Task<SeckillActivities?> GetSeckillActivityAsync(int seckillId)
        {
            string key = $"Seckill:Activity:{seckillId}";
            string? json = await _db.StringGetAsync(key);
            if (string.IsNullOrEmpty(json)) return null;

            return Newtonsoft.Json.JsonConvert.DeserializeObject<SeckillActivities>(json);

        }


        // 获取所有待监控的秒杀活动ID
        public async Task<List<int>> GetAllSeckillActivityIdsAsync()
        {
            var ids = await _db.SortedSetRangeByScoreAsync("Seckill:Activities:All", 0, long.MaxValue);
            return ids.Select(id => int.Parse(id!)).ToList();
        }

        // 更新活动状态（Redis）
        public async Task UpdateSeckillStatusAsync(int seckillId, int? status)
        {
            var seckill = await GetSeckillActivityAsync(seckillId);
            if (seckill == null) return;
            seckill.Status = status;
            await SetSeckillActivityAsync(seckill);
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }

    }
}
