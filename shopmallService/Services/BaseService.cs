using publicClassLibrary.Helpers;
using shopmallService.Interfaces;
using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace shopmallService.Services
{
    /// <summary>
    /// 通用基础服务实现类
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class BaseService : IBaseService
    {


        private readonly SqlSugarHelper _dbHelper;
        public BaseService(SqlSugarHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }





        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        public  T GetById<T>(int id) where T : class, new()
        {
         return _dbHelper.GetById<T>(id);

        }

        /// <summary>
        /// 获取所有实体列表
        /// </summary>
        public List<T> GetAll<T>() where T : class, new()
        {
            return  _dbHelper.GetAll<T>();
        }

        /// <summary>
        /// 根据条件查询实体列表+排序
        /// </summary>
        public  List<T> GetList<T>(Expression<Func<T, bool>> whereLambda, List<OrderByModel> orderbyList = null) where T : class, new()
        {
            var allData = _dbHelper.GetList<T>(whereLambda, orderbyList);
            return allData;

        }

        /// <summary>
        /// 分页查询+排序
        /// </summary>
        public  List<T> GetPageList<T>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, List<OrderByModel> orderbyList = null) where T : class, new()
        {

            var allData =_dbHelper.GetPageList<T>(pageIndex, pageSize, out totalCount,whereLambda, orderbyList);
            return allData;
        }



        /// <summary>
        /// 匿名类条件查询+排序
        /// </summary>
        public List<TResult> GetList<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new()
        {
            var allData = _dbHelper.GetList<T, TResult>(whereLambda, selectExpression, orderbyList);
            return allData;
        }


        /// <summary>
        /// 匿名类分页查询+排序
        /// </summary>
        public List<TResult> GetPageList<T, TResult>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new()
        {
            var allData = _dbHelper.GetPageList<T, TResult>(pageIndex, pageSize, out totalCount, whereLambda, selectExpression, orderbyList);
            return allData;
        }

        /// <summary>
        /// 匿名类输入sql语句查询返回一个数据表
        /// </summary>
        public List<TResult> SqlQuery<TResult>(string sql) where TResult : class, new()
        {

            var allData = _dbHelper.SqlQuery<TResult>(sql);
            return allData;
        }

        /// <summary>
        /// 匿名类输入sql语句查询返回一个数据集
        /// </summary>
        public DataSet SqlQuery(string sql)
        {
            var dataSet = _dbHelper.SqlQuery(sql);
            return dataSet;
        }


        /// <summary>
        /// 匿名类求和和求记录数List<GroupByModel>
        /// </summary>

        public List<TResult> GetSum<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<GroupByModel> groupbyList = null) where T : class, new() where TResult : class, new()
        {
            var allData = _dbHelper.GetSum<T, TResult>( whereLambda, selectExpression, groupbyList);
            return allData;
        }

        /// <summary>
        /// 匿名类求和和求记录数用Expression<Func<T, object>>
        /// </summary>

        public List<TResult> GetSum<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, Expression<Func<T, object>> groupbyExpression) where T : class, new() where TResult : class, new()
        {
            var allData = _dbHelper.GetSum<T, TResult>(whereLambda, selectExpression, groupbyExpression);
            return allData;

        }









        /*
        /// <summary>
        /// 添加实体
        /// </summary>
        public async Task<int> InsertAsync(T entity)
        {
            return await _db.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        public async Task<int> InsertRangeAsync(List<T> entities)
        {
            return await _db.Insertable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public async Task<int> UpdateAsync(T entity)
        {
            return await _db.Updateable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        public async Task<int> DeleteAsync(int id)
        {
            return await _dbHelper.Delete<T>().In(id).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        public async Task<int> DeleteRangeAsync(List<int> ids)
        {
            return await _db.Deleteable<T>().In(ids).ExecuteCommandAsync();
        }

     

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public async Task<int> GetCountAsync(Expression<Func<T, bool>> whereExpression = null)
        {
            var query = _db.Queryable<T>();
            if (whereExpression != null)
            {
                query = query.Where(whereExpression);
            }
            return await query.CountAsync();
        }

        /// <summary>
        /// 是否存在符合条件的记录
        /// </summary>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await _db.Queryable<T>().Where(whereExpression).AnyAsync();
        }

        */




        #region  异步  Method


        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        public async Task<T> GetByIdAsync<T>(int id) where T : class, new()
        {
            return await _dbHelper.GetByIdAsync<T>(id);
        }

        /// <summary>
        /// 获取所有实体列表
        /// </summary>
        public async Task<List<T>> GetAllAsync<T>() where T : class, new()
        {
            return await _dbHelper.GetAllAsync<T>();
        }

        /// <summary>
        /// 根据条件查询实体列表
        /// </summary>
        public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            var allData = await _dbHelper.GetListAsync<T>(whereLambda);
            return allData;

        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<List<T>> GetPageListAsync<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda) where T : class, new()
        {

            var allData = await _dbHelper.GetPageListAsync<T>(pageIndex, pageSize, whereLambda);
            return allData;
        }

        #endregion

    }
}
