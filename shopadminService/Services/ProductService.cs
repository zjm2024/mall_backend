using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using static Dm.parser.SQLProcessor;

namespace shopadminService.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly SqlSugarHelper _dbHelper;
        public ProductService(SqlSugarHelper dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }


        public List<Products> getProductsPageList(int pageIndex, int pageSize, int appType, int? productStatus, out int totalCount)
        {
            //加排序

            List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "ProductId", OrderByType = OrderByType.Desc });

            Expression expbody = null;

            ParameterExpression param = Expression.Parameter(typeof(Products), "it");
            Expression expappType = Expression.Equal(Expression.Property(param, "AppType"), Expression.Constant(appType));

            expbody = expappType;


            if (productStatus != null)
            {
                Expression expstatus = Expression.Equal(Expression.Property(param, "ProductStatus"), Expression.Constant(productStatus));
                expbody = Expression.AndAlso(expappType, expstatus);
            }


            Expression<Func<Products, bool>> lambdaExpression = Expression.Lambda<Func<Products, bool>>(
                expbody,
                param
            );

            var list = GetPageList<Products>(pageIndex, pageSize, out totalCount, lambdaExpression, orderbyList);
            return list;
        }


        /// <summary>
        /// 添加或更新商品
        /// </summary>
        /// <param name="Categories">分类cVO</param>

        public ResultObject updateProducts(Products pV0)
        {
            //判断商品名称是否重复
            string productName = pV0.ProductName;
            int productId = pV0.ProductId;
            var isExist = RecordExist<Products, dynamic>(it => it.ProductName == productName, it => it.ProductId);

            if (isExist)
            {
                return new ResultObject() { Flag = 0, Message = "商品名称已存在!", Result = null };
            }

            if (productId == 0)
            {

                int id = Add<Products>(pV0);
                if (id > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = id };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

            }
            else
            {

                bool isSuccess = Update<Products>(pV0, it => new { it.ProductName, it.ProductContent });
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }

    }
}

