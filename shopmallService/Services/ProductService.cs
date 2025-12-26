using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using shopmallService.Interfaces;
using SqlSugar;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace shopmallService.Services
{

    public class ProductService : BaseService, IProductService
    {
        //private readonly SqlSugarHelper _dbHelper;
        //private readonly ISqlSugarClient _db;
        public ProductService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            //基本查询接口可以直接调用，如果没有特殊情况可以不引用 _dbHelper和 db 

            //_dbHelper = dbHelper;
            //_db = db;
        }
        public DataSet getTokenAll()
        {
            string sql = "select * from T_CSC_Token where 1=1 and  Token = 'bb66fea0-18bc-4562-9b3c-0330931db897'  ;";
            sql += "select * from mall_products ; ";

            //var list = SqlQuery<dynamic>(sql);
            var list = SqlQuery(sql);
            return list;
        }


        public List<products> getProductsAll()
        {

            var list = GetAll<products>();
            return list;

        }
        public products getProductsById(int id)
        {
            var outobj = GetById<products>(id);
            return outobj;
        }

        public List<products> getProductsPageList(int pageIndex, int pageSize, int appType, out int totalCount)
        {
            //加排序

            List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "ProductId", OrderByType = OrderByType.Desc });

            var list = GetPageList<products>(pageIndex, pageSize, out totalCount, it => it.AppType == appType, orderbyList);
            return list;
        }

        public List<products> getProductsList(int appType)
        {
            //加排序

            List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "ProductId", OrderByType = OrderByType.Desc });

            //var list = GetList<products>(it => it.AppType == appType, orderbyList);
            var list = GetList<products>(it => it.AppType == appType);
            return list;
        }


        public List<dynamic> getCustomClumnsProductsList(int appType)
        {
            //加排序

            List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "ProductId", OrderByType = OrderByType.Desc });



            var list = GetList<products, dynamic>(it => it.AppType == appType, it => new { it.ProductId, it.ProductName, it.ProductImage }, orderbyList);
            //var list = GetList<products,dynamic>(it => it.AppType == appType,it=>new { it.ProductId, it.ProductName, it.ProductImage });
            return list;
        }

        public List<dynamic> getCustomClumnsProductsPageList(int pageIndex, int pageSize, int appType, out int totalCount)
        {
            //加排序

            List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "ProductId", OrderByType = OrderByType.Desc });

            // var list = GetPageList<products,dynamic>(pageIndex, pageSize, out totalCount, it => it.AppType == appType,   it=>new {it.ProductId,it.ProductName ,it.ProductImage}, orderbyList);
             var list = GetPageList<products,dynamic>(pageIndex, pageSize, out totalCount, it => it.AppType == appType,   it=>new {it.ProductId,it.ProductName ,it.ProductImage});
            return list;
        }

    }
}
