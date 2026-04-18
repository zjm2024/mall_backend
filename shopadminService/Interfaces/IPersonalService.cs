using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopadminService.Interfaces
{
    public interface IPersonalService
    {

        ResultObject getPersonalPageList(int pageIndex, int pageSize, int appType,int businessId, string? searchKey, int? status);

        ResultObject updatePersonal(Personal pV0, string[] updateColums = null);

        ResultObject getPersonalById(int id);

        ResultObject deletePersonal(int id);


    }
}
