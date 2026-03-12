using publicClassLibrary.Entitys;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Models;


namespace shopmallService.Interfaces
{
    public interface IProductService:IBaseService
    {


        ResultObject getInfoList(int businessId, int appType);

        ResultObject getProductsPageList(int pageIndex, int pageSize, string treePath, int businessId ,int appType, out int totalCount);

        ResultObject getProductsById(int productId);

        ResultObject updateCategories(Categories cV0);

        ResultObject getCardByShare(int appType);

        ResultObject getSeckillTimersList(int appType);

        ResultObject getCurDateSeckillList(string timer,int appType);

        /*
        List<products> getProductsAll();

      



        List<products> getProductsList(int appType);

        List<dynamic> getCustomClumnsProductsList(int appType);

        List<dynamic> getCustomClumnsProductsPageList(int pageIndex, int pageSize, int appType, out int totalCount);

        List<dynamic> getProductSum(int appType);

  */


    }
}
