using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopadminService.Interfaces
{
    public interface IDataDictService
    {

        ResultObject getDataDictPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status);

        Task<ResultObject> updateDataDict(DataDicts bV0, string[] updateColums = null);

        ResultObject getDataDictById(int id);

        Task<ResultObject> deleteDataDict(int id);


    }
}
