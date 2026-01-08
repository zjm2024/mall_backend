using publicClassLibrary.Entitys;

namespace shopadminService.Interfaces
{
    public interface IOrderService
    {
        
      List<Orders> getOrdersPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? orderStatus, out int totalCount);


    }
}
