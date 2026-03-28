using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopadminService.Interfaces
{
    public interface IOrderService
    {

        ResultObject getOrdersPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? orderStatus);

        ResultObject updateOrders(Orders oV0, string[] updateColums = null);

        ResultObject getOrdersById(int id);

        ResultObject getOrdersSubsPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? orderStatus);


        ResultObject getOrdersSubsById(int id);
    }
}
