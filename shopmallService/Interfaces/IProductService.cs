using publicClassLibrary.Entitys;
using System.Data;
using System.Linq.Expressions;

namespace shopmallService.Interfaces
{
    public interface IProductService:IBaseService
    {
        Dictionary<string, object> getTokenAll();
        List<products> getProductsAll();

        products getProductsById(int id);

        List<products> getProductsPageList(int pageIndex, int pageSize,int appType,out int totalCount);

        List<products> getProductsList(int appType);

        List<dynamic> getCustomClumnsProductsList(int appType);

        List<dynamic> getCustomClumnsProductsPageList(int pageIndex, int pageSize, int appType, out int totalCount);

        List<dynamic> getProductSum(int appType);

  
   

    }
}
