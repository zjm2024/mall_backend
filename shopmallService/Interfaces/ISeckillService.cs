using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopmallService.Interfaces
{
    public interface ISeckillService
    {
        ResultObject updateSeckillStatus(SeckillActivities sV0, string[] updateColums = null);

        Task saveHotSeckillProductsAsync(int businessId, DateTime seckillTime);

        Task getHotSeckillProductsAsync(int businessId, DateTime seckillTime);
        
        
        
  

        Task<bool> saveAllSeckillTimesAsync();      //没有使用
    }
}
