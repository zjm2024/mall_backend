using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopmallService.Interfaces;
using SqlSugar;

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

        public List<Categories> getCategoriesList(int appType)
        {
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
              new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });
            var list = GetList<Categories>(it => it.AppType == appType && it.Status == 1, orderbyList);
            return list;
        }


        /// <summary>
        /// 添加或更新分类
        /// </summary>
        /// <param name="Categories">分类cVO</param>

        public ResultObject updateCategories(Categories cVO)
        {
            //判断分类名称是否重复
            string categoryName = cVO.CategoryName;
            int categoryId = cVO.CategoryId;
            var exists = RecordExist<Categories, dynamic>(it => it.CategoryName == categoryName, it => it.CategoryId);

            if (exists)
            {
                return new ResultObject() { Flag = 0, Message = "分类名称已存在!", Result = null };
            }

            if (categoryId == 0)
            {

                int id = Add<Categories>(cVO);
                if (id > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = id };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

            }
            else
            {

                bool isSuccess = Update<Categories>(cVO, it => new { it.CategoryName, it.Icon });
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }




        /*
        public Dictionary<string, object> getTokenAll()
        {
            string sql = "select * from T_CSC_Token where 1=1 and  Token = 'bb66fea0-18bc-4562-9b3c-0330931db897'  ;";
            sql += "select * from mall_products ; ";

            var result = new Dictionary<string, object>();

            //var list = SqlQuery<dynamic>(sql);
            var ds = SqlQuery(sql);
            for (int i = 0;i<ds.Tables.Count;i++)
            {
                var table = ds.Tables[i];
                result[table.TableName] = MapTableToList(table);
            }
            return result;
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

        public List<dynamic> getProductSum(int appType)
        {
            List<GroupByModel> groupbyList = GroupByModel.Create(
            new GroupByModel() { FieldName = "CategoryId" });
            //var list = GetSum<products, dynamic>(it => it.AppType == appType, it => new { it.CategoryId, TotalStock=SqlFunc.AggregateSum(it.TotalStock),TotalCount= SqlFunc.AggregateCount(it.ProductId) }, groupbyList);
            //var list = GetSum<products, dynamic>(it => it.AppType == appType, it => new {  TotalStock=SqlFunc.AggregateSum(it.TotalStock),TotalCount= SqlFunc.AggregateCount(it.ProductId) });
            var list = GetSum<products, dynamic>(it => it.AppType == appType, it => new { it.CategoryId, TotalStock = SqlFunc.AggregateSum(it.TotalStock), TotalCount = SqlFunc.AggregateCount(it.ProductId) },it=>it.CategoryId);
            return list;
           
        }

        private  List<dynamic> MapTableToList(DataTable dataTable)
        {
            // 空值校验
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable), "DataTable 不能为空");

            var dynamicList = new List<dynamic>();

            // 遍历每一行
            foreach (DataRow row in dataTable.Rows)
            {
                // 使用 ExpandoObject 实现动态对象（.NET 内置的动态对象类型）
                dynamic dynamicObj = new ExpandoObject();
                var expandoDict = (IDictionary<string, object>)dynamicObj;

                // 遍历当前行的所有列
                foreach (DataColumn column in dataTable.Columns)
                {
                    // 获取列值，处理 DBNull 转换为 null
                    object value = row[column] == DBNull.Value ? null : row[column];
                    // 添加列名和值到动态对象
                    expandoDict[column.ColumnName] = value;
                }

                // 将动态对象添加到列表
                dynamicList.Add(dynamicObj);
            }

            return dynamicList;
        }

        */
    }
}
