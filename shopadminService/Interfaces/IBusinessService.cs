using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopadminService.Interfaces
{
    public interface IBusinessService
    {

        ResultObject getBusinessPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status);

        ResultObject updateBusiness(Business bV0, string[] updateColums = null);

        ResultObject getBusinessById(int id);

        ResultObject deleteBusiness(int id);


    }
}
