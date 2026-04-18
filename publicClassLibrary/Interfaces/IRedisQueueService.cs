using publicClassLibrary.Entitys;
using StackExchange.Redis;

namespace publicClassLibrary.Interfaces
{
    public interface IRedisQueueService
    {


        Task<string?> GetStringAsync(string key);

        Task SetStringAsync(string key, string value, TimeSpan? expiry = null);


        Task<bool> DelKeyAsync(string key);

        Task<List<string>> ScanKeysAsync(string pattern, int pageSize = 1000);

        Task<List<string>> GetKeysAsync(string pattern);


        Task<bool> PublishOrderAsync(Orders order);

        Task<Orders> ConsumeOrderAsync();



        Task CreateBatchSeckillActivityAsync(string seckillKey, List<SeckillActivities> seckills);

        Task SetSeckillActivityAsync(SeckillActivities seckill);

        Task<SeckillActivities?> GetSeckillActivityAsync(int seckillId);

        Task<List<int>> GetAllSeckillActivityIdsAsync();

        Task UpdateSeckillStatusAsync(int seckillId, int? status);
    }
}
