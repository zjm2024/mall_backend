using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopadminService.Interfaces
{
    public interface IOrderService
    {

        List<Orders> getOrdersPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? orderStatus, out int totalCount);

        ResultObject updateOrders(Orders oV0, string[] updateColums = null);

        Orders getOrdersById(int id);


    }
}
