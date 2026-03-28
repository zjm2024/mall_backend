using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopadminService.Interfaces
{
    public interface ISeckillService
    {

        ResultObject getSeckillPageList(int pageIndex, int pageSize, int appType,int businessId, string? searchKey, int? status);

        ResultObject updateSeckill(SeckillActivities sV0, string[] updateColums = null);

        Task<ResultObject> checkSeckill(SeckillActivities sV0, string[] updateColums = null);

        ResultObject getSeckillById(int id);

        ResultObject getTimeOptions(int appType);


    }
}
