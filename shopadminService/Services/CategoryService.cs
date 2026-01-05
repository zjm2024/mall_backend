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
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly SqlSugarHelper _dbHelper;
        public CategoryService(SqlSugarHelper dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<Categories> getCategoriesList(int appType, int? status)
        {
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
              new OrderByModel() { FieldName = "SortOrder", OrderByType = OrderByType.Asc });

            Expression expbody = null;

            ParameterExpression param = Expression.Parameter(typeof(Categories), "it");
            Expression expappType = Expression.Equal(Expression.Property(param, "AppType"), Expression.Constant(appType));

            expbody = expappType;


            if (status!=null)
            {
                Expression expstatus = Expression.Equal(Expression.Property(param, "Status"), Expression.Constant(status));
                expbody = Expression.AndAlso(expappType, expstatus);
            }


            Expression<Func<Categories, bool>> lambdaExpression = Expression.Lambda<Func<Categories, bool>>(
                expbody,
                param
            );

            //var list = GetList<Categories>(it => it.AppType == appType && it.Status == status, orderbyList);
            var list = GetList<Categories>(lambdaExpression, orderbyList);
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

    }
}

