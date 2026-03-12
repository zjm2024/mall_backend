using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopmallService.Interfaces;
using SqlSugar;
using StackExchange.Redis;
using System.Security.AccessControl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace shopmallService.Services
{

    public class ProductService : BaseService, IProductService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public ProductService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            //基本查询接口可以直接调用，如果没有特殊情况可以不引用 _dbHelper和 db 
            _dbHelper = dbHelper;
            _db = db;
        }

        public ResultObject getInfoList( int businessId, int appType)
        {
            try
            {
                //加排序

                List<OrderByModel> orderbyList = OrderByModel.Create(
                new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

                /*
                var categoriesList = GetList<Categories,dynamic>(it => it.BusinessId == businessID && it.AppType == appType && it.ParentId == 0 && it.Status == 1,
                 it => new { it.CategoryId, it.CategoryName, it.Icon, it.SortOrder },
                orderbyList);
                */


                //加查询条件
                var conModels = new List<IConditionalModel>();
                conModels.add(new ConditionalModel { FieldName = "AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });
                conModels.add(new ConditionalModel { FieldName = "BusinessId", ConditionalType = ConditionalType.Equal, FieldValue = businessId.toString() });
                conModels.add(new ConditionalModel { FieldName = "Status", ConditionalType = ConditionalType.Equal, FieldValue = "1" });

                //查询树状结构
                var categoriesList = _db.Queryable<Categories>().Where(conModels).OrderBy(orderbyList).ToTree(it => it.Children, it => it.ParentId, 0, it => it.CategoryId).ToList();


                object res = new
                {
                    CategoriesList = categoriesList,

                };

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }


        public ResultObject getProductsPageList(int pageIndex, int pageSize, string treePath, int businessId, int appType, out int totalCount)
        {
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

            //查询当前分类的产品
            var conModels1 = new List<IConditionalModel>();
            conModels1.add(new ConditionalModel { FieldName = "BusinessId", ConditionalType = ConditionalType.Equal, FieldValue = businessId.toString() });
            conModels1.add(new ConditionalModel { FieldName = "TreePath", ConditionalType = ConditionalType.LikeLeft, FieldValue = treePath.toString() });
            conModels1.add(new ConditionalModel { FieldName = "ProductStatus", ConditionalType = ConditionalType.Equal, FieldValue = "1" });


            var allProductsList = GetPageList<Products, dynamic>(pageIndex, pageSize, out totalCount, conModels1,

                   it => new
                   {
                       ProductId = it.ProductId,
                       ProductName = it.ProductName,
                       ProductImage = it.ProductImage,
                       CurrentPrice = it.CurrentPrice,
                       OriginalPrice = it.OriginalPrice,
                       Sales = it.Sales,
                       PerPersonLimit = it.PerPersonLimit,
                       CategoryId = it.CategoryId,
                       ProductStatus = it.ProductStatus,
                       TreePath=it.TreePath,
                       CreateTime = SqlFunc.ToString(it.CreateTime)
                   },
                   orderbyList);

            object res = new
            {
                AllProductsList = allProductsList

            };

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
        }


        public ResultObject getProductsById(int productId)
        {
            var outobj = GetById<ViewProducts>(productId);
            //带上规格
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
              new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

            var productSpecs = GetList<ProductSpecs>(it => it.ProductId == productId, orderbyList);
            outobj.ProductSpecs = productSpecs;

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj };
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

        /// <summary>
        /// 获取商城首页数据
        /// </summary>
        public ResultObject getCardByShare(int appType)
        {
            try
            {
                List<OrderByModel> orderbyList = OrderByModel.Create(
                new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

                //默认选择8条分类数据
                int pageIndex = 1;
                int pageSize = 8;
                int totalCount;

                var categoriesList = GetPageList<Categories, dynamic>(pageIndex, pageSize, out totalCount, it => it.AppType == appType && it.ParentId == 0 && it.Status == 1,
                    it => new { it.CategoryId, it.CategoryName, it.Icon, it.SortOrder },
                    orderbyList);


                //轮播商品
                var bannerProductList= GetList<ViewProducts, dynamic>(it =>  it.AppType == appType && it.BannerProduct == 1 && it.ProductStatus == 1,
                  it => new {it.BusinessId, it.BusinessName, it.ProductId, it.ProductName, it.ProductImage, it.CurrentPrice, it.OriginalPrice, it.Sales, it.PerPersonLimit, it.SortOrder });



                //推荐商品
                var hostProductList = GetList<ViewProducts, dynamic>(it =>  it.AppType == appType && it.HotProduct == 1 && it.ProductStatus == 1,
                  it => new {it.BusinessId,it.BusinessName,it.ProductId, it.ProductName, it.ProductImage, it.CurrentPrice, it.OriginalPrice, it.Sales, it.PerPersonLimit, it.SortOrder });


                //秒杀商品
                var seckillProductList = _db.Queryable<SeckillActivities>()
                .LeftJoin<Products>((s, p) => s.ProductId == p.ProductId)
                .LeftJoin<Business>((s,p, b)=>s.BusinessId==b.BusinessId)
                .Where(s =>  s.AppType == appType && s.Status == 1 && s.EndTime>DateTime.Now)
                .Select((s,p,b) => new
                {
                    SeckillId = s.SeckillId,
                    BusinessId = s.BusinessId,
                    BusinessName = b.BusinessName,
                    ProductId = s.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImage,
                    SeckillPrice = s.SeckillPrice,
                    OriginalPrice = p.OriginalPrice,
                    ActivityStock = s.ActivityStock,
                    UsedStock = s.UsedStock,
                    SoldPercent = s.SoldPercent,
                    PerPersonLimit = s.PerPersonLimit,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .ToList();


                object res = new
                {
                    CategoriesList = categoriesList,
                    BannerProductList= bannerProductList,
                    HostProductList = hostProductList,
                    SeckillProductList = seckillProductList,

                };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }

            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }


        public ResultObject getSeckillTimersList(int appType)
        {
            try
            {
                //秒杀时间段
                var seckillTimerList = GetList<SeckillTimers, dynamic>(it => it.AppType == appType, it => new { it.TimerId, it.SeckillTime, it.SeckillMinutes, it.SortOrder }).OrderBy(it => it.SortOrder).ToList();
                object res = new
                {
                    seckillTimerList = seckillTimerList

                };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public ResultObject getCurDateSeckillList(string timer, int appType)
        {
            try
            {

                List<OrderByModel> orderbyList = OrderByModel.Create(
                new OrderByModel() { FieldName = "EndTime", OrderByType = OrderByType.Asc });

                var curDateTime = DateTime.Today.ToShortDateString() + " " + timer;
                //秒杀商品
                var seckillProductList = _db.Queryable<SeckillActivities>()
                .LeftJoin<Products>((s, p) => s.ProductId == p.ProductId)
                .LeftJoin<Business>((s, p, b) => s.BusinessId == b.BusinessId)
                .Where(s => s.AppType == appType  && SqlFunc.ToDate(s.StartTime) == SqlFunc.ToDate(curDateTime))
                .Select((s, p, b) => new
                {
                    SeckillId = s.SeckillId,
                    BusinessId = s.BusinessId,
                    BusinessName = b.BusinessName,
                    ProductId = s.ProductId,
                    ProductName = p.ProductName,
                    ProductImage = p.ProductImage,
                    SeckillPrice = s.SeckillPrice,
                    OriginalPrice = p.OriginalPrice,
                    ActivityStock = s.ActivityStock,
                    UsedStock = s.UsedStock,
                    SoldPercent = s.SoldPercent,
                    PerPersonLimit = s.PerPersonLimit,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Status=s.Status
                })
                .ToList();

                object res = new
                {
                    SeckillProductList = seckillProductList,

                };
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        /*
     

        public List<products> getProductsAll()
        {

            var list = GetAll<products>();
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
