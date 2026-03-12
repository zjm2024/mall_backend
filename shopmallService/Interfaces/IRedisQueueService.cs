using publicClassLibrary.Entitys;

namespace shopmallService.Interfaces
{
    public interface IRedisQueueService
    {
       Task<bool> PublishOrderAsync(Orders order);

        Task<Orders> ConsumeOrderAsync();
    }
}
