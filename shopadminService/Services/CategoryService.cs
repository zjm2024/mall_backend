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
        public CategoryService(SqlSugarHelper dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<Categories> getCategoriesList(int appType, string? categoryName,int? status)
        {
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
              new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

            //加查询条件
           var conditions = new List<IConditionalModel>();
         
            conditions.add(new ConditionalModel
            {
                FieldName = "AppType",
                FieldValue = appType.toString(),
                ConditionalType = ConditionalType.Equal
            }); 

            if (categoryName != null)
            {
                conditions.add(new ConditionalModel
                {
                    FieldName = "CategoryName",
                    FieldValue = categoryName.toString(),
                    ConditionalType = ConditionalType.Like
                });
            }

            if (status != null)
            {
                conditions.add(new ConditionalModel
                {
                    FieldName = "Status",
                    FieldValue = status.toString(),
                    ConditionalType = ConditionalType.Equal
                });
            }


            var list = GetList<Categories>(conditions, orderbyList);
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

                bool isSuccess = Update<Categories>(cVO, it => new { it.CategoryName, it.Icon,it.SortOrder,it.Status });
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
                    return new ResultObject() { Flag = 1, Message = "部分删除成功!", Result = null };
                }



            }
            catch
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
            }
        }
    }
}

