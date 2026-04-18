using Dm.util;
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

        /// <summary>
        /// 模糊匹配Key（基于SCAN，非阻塞，推荐生产环境使用）
        /// </summary>
        /// <param name="pattern">匹配模式，如 "user:*"</param>
        /// <param name="pageSize">每次扫描的数量</param>
        /// <returns>匹配的Key集合</returns>
        public async Task<List<string>> ScanKeysAsync(string pattern, int pageSize = 1000)
        {
            var keys = new List<string>();

            // 获取所有终结点（针对集群环境）
            var endPoints = _redis.GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                var server = _redis.GetServer(endPoint);

                // 使用 Scan 方法，它底层基于 SCAN 命令，不会阻塞服务器
                // 注意：Scan 返回的是 IEnumerable<RedisKey>，是懒加载的
                var redisKeys = server.Keys(pattern: pattern, pageSize: pageSize);

                foreach (var key in redisKeys)
                {
                    keys.Add(key.ToString());
                }
            }
            return keys;
            //return keys.Distinct(); // 集群环境下可能需要去重

        }

        /// <summary>
        /// 使用 Keys 命令（阻塞式，仅建议开发/测试或小数据量使用）
        /// </summary>
        public async Task<List<string>> GetKeysAsync(string pattern)
        {
            var keys = new List<string>();
            var endPoints = _redis.GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                var server = _redis.GetServer(endPoint);
                // Keys 方法会根据 Redis 版本自动选择 KEYS 或 SCAN，但在大数据量下仍需谨慎
                var redisKeys = server.Keys(pattern: pattern);

                foreach (var key in redisKeys)
                {
                    keys.Add(key.ToString());
                }
            }

            return keys;
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




        //创建批量秒杀活动
        public async Task CreateBatchSeckillActivityAsync(string seckillKey,List<SeckillActivities> seckills)
        {
            //  使用 Redis Pipeline 批量写入，减少网络往返
            var batch = _db.CreateBatch();
            var tasks = new List<Task>();

            foreach (var product in seckills)
            {
                var productKey = $"{seckillKey}:product:{product.ProductId}";

                // Hash 存储商品详情
                var hashEntries = new HashEntry[]
                {
                new("seckillId", product.SeckillId),
                new("productNo", product.ProductNo),
                new("productId", product.ProductId),
                new("productName", product.ProductName),
                new("seckillPrice", product.SeckillPrice.ToString()),
                new("activityStock", product.ActivityStock.ToString()),
                new("usedStock", product.UsedStock.ToString()),
               // new("soldPercent", product.SoldPercent.ToString()),
                new("startTime", product.StartTime.ToString("O")),
                new("endTime", product.EndTime.ToString("O")),
                new("status", "0") // 0=未开始, 1=进行中, 2=已结束
                };

                tasks.Add(batch.HashSetAsync(productKey, hashEntries));

                // 初始化库存（使用 Redis String 原子操作）
                tasks.Add(batch.StringSetAsync($"{productKey}:stock", product.ActivityStock));

                // 初始化已售数量
                tasks.Add(batch.StringSetAsync($"{productKey}:sold", 0));
            }

            //  存储商品列表索引（SortedSet，按排序字段）
            var sortedSetEntries = seckills
                .Select(p => new SortedSetEntry(p.ProductId, p.SeckillId))
                .ToArray();
            tasks.Add(batch.SortedSetAddAsync($"{seckillKey}:products", sortedSetEntries));

            // 4. 设置整体过期时间（24小时后自动清理）
            tasks.Add(batch.KeyExpireAsync($"{seckillKey}:products", TimeSpan.FromHours(24)));

            batch.Execute();
            await Task.WhenAll(tasks);
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
