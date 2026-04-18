using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopmallService.Interfaces
{
    public interface ISeckillService
    {
        ResultObject updateSeckillStatus(SeckillActivities sV0, string[] updateColums = null);

        Task hotSeckillProductsAsync(string businessId, string seckillTime);

        Task<bool> saveAllSeckillTimesAsync();
    }
}
