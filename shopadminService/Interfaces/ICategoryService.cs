using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using shopadminService.Services;

namespace shopadminService.Interfaces
{
    public interface ICategoryService 
    {
       List<Categories> getCategoriesList(int appType, string? categoryName,int? status);

        ResultObject updateCategories(Categories cV0,string[] updateColums=null);

        ResultObject deleteCategories(int id);

    }
}
