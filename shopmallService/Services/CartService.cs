using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopmallService.Interfaces;
using SqlSugar;
using System.Text;

namespace shopmallService.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly SqlSugarHelper _dbHelper;
        public CartService(SqlSugarHelper dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public ResultObject getCartByIndex(int personalId,int appType)
        {
            try
            {
                //推荐商品
                var hostProductList = GetList<Products, dynamic>(it =>  it.AppType == appType && it.HotProduct == 1 && it.ProductStatus == 1,
                  it => new { it.ProductId, it.ProductName, it.ProductImage, it.CurrentPrice, it.OriginalPrice, it.Sales, it.PerPersonLimit, it.SortOrder });

                object res = new
                {
                    HostProductList = hostProductList,
                    //SeckillProductList = seckillProductList,

                };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }
    }
}
