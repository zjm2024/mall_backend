using Dm.util;
using NetTaste;
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
   
    public class CategoryService : BaseService, ICategoryService
    {

        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public CategoryService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
        }

        public List<Categories> getCategoriesList(int appType, string? categoryName, int? status)
        {
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
              new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

            //加查询条件
            var conModels = new List<IConditionalModel>();

            conModels.add(new ConditionalModel { FieldName = "AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });

            if (categoryName != null)
            {
                conModels.add(new ConditionalModel { FieldName = "CategoryName", ConditionalType = ConditionalType.Like, FieldValue = categoryName.toString() });
            }

            if (status != null)
            {
                conModels.add(new ConditionalModel { FieldName = "Status", ConditionalType = ConditionalType.Equal, FieldValue = status.toString() });
            }

   

            //查询树状结构
            var list =  _db.Queryable<Categories>().Where(conModels).OrderBy(orderbyList).ToTree(it => it.Children, it => it.ParentId, 0, it => it.CategoryId).ToList();


            return list;



        }


        /// <summary>
        /// 添加或更新分类
        /// </summary>
        /// <param name="Categories">分类cVO</param>

        public ResultObject updateCategories(Categories cVO, string[] updateColums = null)
        {
            //判断分类名称是否重复
            string categoryName = cVO.CategoryName;
            int categoryId = cVO.CategoryId;
            if (categoryId == 0)
            {
                var isExist = RecordExist<Categories, dynamic>(it => it.CategoryName == categoryName, it => it.CategoryId);

                if (isExist)
                {
                    return new ResultObject() { Flag = 0, Message = "分类名称已存在!", Result = null };
                }
            }

            if (categoryId == 0)
            {

                int id = Add<Categories>(cVO);
                cVO.CategoryId = id;
                if (id > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = cVO };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

            }
            else
            {
                //it => new { it.CategoryName, it.Icon,it.SortOrder,it.Status }
                bool isSuccess = Update<Categories>(cVO, updateColums);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = cVO };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }


        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id">分类id</param>
        public ResultObject deleteCategories(int id)
        {
            try
            {

                bool isSuccess = Delete<Categories>(id);
                if (isSuccess)
                {
                    return new ResultObject() { Flag = 1, Message = "删除成功!", Result = id };
                }
                else
                {
                    return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }



            }
            catch(Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!"+ ex.toString(), Result = null };
            }
        }
    }
}

