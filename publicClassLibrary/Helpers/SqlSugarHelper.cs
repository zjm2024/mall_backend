using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Helpers
{
    public class SqlSugarHelper
    {
        private readonly ISqlSugarClient _db;
        public SqlSugarHelper(ISqlSugarClient db)
        {
            _db = db;
        }

        public bool CreateTable(Type type)
        {
            try
            {
                _db.CodeFirst.InitTables(type);

                return true;

            }
            catch (Exception ex)
            {
                // 记录日志或提示用户
                return false;
            }
        }




        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        public T GetById<T>(int id) where T : class, new()
        {
            return (T)_db.Queryable<T>().InSingle(id);
        }


        /// <summary>
        /// 获取所有数据 不推荐使用 除非表记录很少的情况下允许查询
        /// </summary>
        public List<T> GetAll<T>() where T : class, new()
        {
            return _db.Queryable<T>().ToList();
        }

        /// <summary>
        /// 条件查询+排序
        /// </summary>
        public List<T> GetList<T>(Expression<Func<T, bool>> whereLambda, List<OrderByModel> orderbyList  = null) where T : class, new()
        {
            if (orderbyList == null)
                return _db.Queryable<T>().Where(whereLambda).ToList();
            else
                return _db.Queryable<T>().Where(whereLambda).OrderBy(orderbyList).ToList();
        }


        /// <summary>
        /// 分页查询+排序
        /// </summary>
        public List<T> GetPageList<T>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, List<OrderByModel> orderbyList = null) where T : class, new()
        {
            PageModel pageModel = new();
            pageModel.PageSize = pageSize;
            pageModel.PageIndex = pageIndex;
            totalCount = pageModel.TotalCount;
            if (orderbyList == null)
                return _db.Queryable<T>().Where(whereLambda).ToPageList(pageModel.PageIndex, pageModel.PageSize, ref totalCount);
            else
                return _db.Queryable<T>().Where(whereLambda).OrderBy(orderbyList).ToPageList(pageModel.PageIndex, pageModel.PageSize, ref totalCount);

        }


        /// <summary>
        /// 匿名类条件查询+排序
        /// </summary>
        public List<TResult> GetList<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new()
        {
            if (orderbyList == null)
                return _db.Queryable<T>().Where(whereLambda).Select(selectExpression).ToList();
            else
                return _db.Queryable<T>().Where(whereLambda).OrderBy(orderbyList).Select(selectExpression).ToList();
        }

        /// <summary>
        /// 匿名类分页查询+排序
        /// </summary>
        public List<TResult> GetPageList<T, TResult>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new()
        {
            PageModel pageModel = new();
            pageModel.PageSize = pageSize;
            pageModel.PageIndex = pageIndex;
            totalCount = pageModel.TotalCount;
            if (orderbyList == null)
                return _db.Queryable<T>().Where(whereLambda).Select(selectExpression).ToPageList(pageModel.PageIndex, pageModel.PageSize, ref totalCount);
            else
                return _db.Queryable<T>().Where(whereLambda).Select(selectExpression).OrderBy(orderbyList).ToPageList(pageModel.PageIndex, pageModel.PageSize, ref totalCount);

        }

        /// <summary>
        /// 匿名类输入sql语句查询返回一个数据表
        /// </summary>

        public List<TResult> SqlQuery<TResult>(string sql) where TResult : class, new()
        {
            List<TResult> list = _db.Ado.SqlQuery<TResult>(sql).ToList();
            return list;
 
        }

        /// <summary>
        /// 匿名类输入sql语句查询返回一个数据集
        /// </summary>
        public DataSet SqlQuery(string sql)
        {
            DataSet ds = _db.Ado.GetDataSetAll(sql);
            return ds;
        }

        /// <summary>
        /// 匿名类求和和求记录数用List<GroupByModel>
        /// </summary>

        public List<TResult> GetSum<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<GroupByModel> groupbyList = null) where T : class, new() where TResult : class, new()
        {
            if (groupbyList == null)
                return _db.Queryable<T>().Where(whereLambda).Select(selectExpression).ToList();
            else
                return _db.Queryable<T>().Where(whereLambda).GroupBy(groupbyList).Select(selectExpression).ToList();


        }
        /// <summary>
        /// 匿名类求和和求记录数用Expression<Func<T, object>>
        /// </summary>

        public List<TResult> GetSum<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, Expression<Func<T, object>> groupbyExpression) where T : class, new() where TResult : class, new()
        {
                return _db.Queryable<T>().Where(whereLambda).GroupBy(groupbyExpression).Select(selectExpression).ToList();
        }




        // 增加实体

        public int Add<T>(T entity) where T : class, new()
        {
            try
            {
                int Id = _db.Insertable(entity).ExecuteReturnIdentity();
                return Id;
            }
            catch (Exception ex)
            {
                // 记录日志或提示用户
                return 0;
            }
        }


        // 更新实体
        public bool Update<T>(T entity) where T : class, new()
        {
            try
            {
                _db.Updateable(entity).ExecuteCommand();
                return true;
            }
            catch (Exception ex)
            {
                // 记录日志或提示用户
                return false;
            }
        }



        // 删除实体
        public bool Delete<T>(T entity, Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            try
            {

                _db.Deleteable(entity).Where(whereLambda).ExecuteCommand();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


   






        #region  异步  Method

        /// <summary>
        /// 根据主键获取实体(异步)
        /// </summary>
        public async Task<T> GetByIdAsync<T>(int id) where T : class, new()
        {
            return await _db.Queryable<T>().InSingleAsync(id);
        }

        /// <summary>
        /// 获取所有数据(异步)
        /// </summary>
        public async Task<List<T>> GetAllAsync<T>() where T : class, new()
        {
            return await _db.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 条件查询(异步)
        /// </summary>
        public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            return await _db.Queryable<T>().Where(whereLambda).ToListAsync();
        }


        /// <summary>
        /// 分页查询(异步)
        /// </summary>
        public async Task<List<T>> GetPageListAsync<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            PageModel pageModel = new();
            pageModel.PageSize = pageSize;
            pageModel.PageIndex = pageIndex;
            var result = await _db.Queryable<T>().Where(whereLambda).ToPageListAsync(pageModel.PageIndex, pageModel.PageSize, pageModel.TotalCount);
            return result;
        }

        #endregion


    }
}

