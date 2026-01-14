using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using shopadminService.Services;

namespace shopadminService.Interfaces
{
    public interface IProductService
    {
        List<Products> getProductsPageList(int pageIndex, int pageSize, int appType, int businessId, string? productName, int? productStatus,string? categoryIds, out int totalCount);

        ResultObject updateProducts(Products pV0, string[] updateColums = null);

        Products getProductsById(int id);
        List<Categories> getCategoriesOptions(int appType, int businessId);

    }
}
