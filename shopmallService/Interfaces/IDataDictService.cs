using publicClassLibrary.Entitys;
using publicClassLibrary.Models;

namespace shopmallService.Interfaces
{
    public interface IDataDictService
    {
         Task<ResultObject> getDataDictByCode(string code);
    }
}
