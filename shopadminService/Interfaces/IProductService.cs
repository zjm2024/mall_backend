using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using shopadminService.Services;

namespace shopadminService.Interfaces
{
    public interface IProductService
    {
        List<Products> getProductsPageList(int pageIndex, int pageSize, int appType, int businessId, string? productName, int? productStatus,string? categoryIds, out int totalCount);

        ResultObject updateProducts(Products pV0, string[] updateColums = null, string delSpecsids = "");

        ResultObject deleteProducts(int id);

        ResultObject deleteBatchProducts(string ids);

        Products getProductsById(int id);

        ResultObject deleteProductSpecs(int id);

        ResultObject deleteBatchProductSpecs(string ids);



        List<Categories> getCategoriesOptions(int appType, int businessId);

    }
}
