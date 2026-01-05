using publicClassLibrary.Entitys;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Models;


namespace shopmallService.Interfaces
{
    public interface IProductService:IBaseService
    {

        
        List<Categories> getCategoriesList(int appType);

        ResultObject updateCategories(Categories cV0);
        /*
        Dictionary<string, object> getTokenAll();
        List<products> getProductsAll();

        products getProductsById(int id);

        List<products> getProductsPageList(int pageIndex, int pageSize,int appType,out int totalCount);

        List<products> getProductsList(int appType);

        List<dynamic> getCustomClumnsProductsList(int appType);

        List<dynamic> getCustomClumnsProductsPageList(int pageIndex, int pageSize, int appType, out int totalCount);

        List<dynamic> getProductSum(int appType);

  */


    }
}
