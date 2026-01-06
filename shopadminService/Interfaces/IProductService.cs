using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using shopadminService.Services;

namespace shopadminService.Interfaces
{
    public interface IProductService
    {
        List<Products> getProductsPageList(int pageIndex, int pageSize, int appType, int? productStatus, out int totalCount);

        ResultObject updateProducts(Products pV0);

    }
}
