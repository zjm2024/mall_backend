using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopadminService.Interfaces
{
    public interface IShopService
    {

        ResultObject getShopsPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status);

        ResultObject updateShops(Business bV0, string[] updateColums = null);

        ResultObject getShopsById(int id);

        ResultObject deleteShops(int id);


    }
}
