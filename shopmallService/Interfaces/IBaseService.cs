using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace shopmallService.Interfaces
{
    // 通用基础接口定义
    public interface IBaseService
    {


        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>实体对象</returns>
        T GetById<T>(int id) where T : class, new();

        /// <summary>
        /// 获取所有实体列表 不推荐使用 除非表记录很少的情况下允许查询
        /// </summary>
        /// <returns>实体列表</returns>
        List<T> GetAll<T>() where T : class, new();

        /// <summary>
        /// 根据条件查询实体列表+排序
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <param name="orderExpression">排序表达式</param>
        /// <returns>符合条件的实体列表</returns>
        List<T> GetList<T>(Expression<Func<T, bool>> whereLambda, List<OrderByModel> orderbyList=null) where T : class, new();


        /// <summary>
        /// 分页查询+排序
        /// </summary>
        List<T> GetPageList<T>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, List<OrderByModel> orderbyList = null ) where T : class, new();



        /// <summary>
        /// 匿名类条件查询+排序
        /// </summary>
        List<TResult> GetList<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new();


        /// <summary>
        /// 匿名类分页查询+排序
        /// </summary>
        List<TResult> GetPageList<T, TResult>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new();

        /// <summary>
        /// 匿名类输入sql语句查询返回一个数据表
        /// </summary>
        List<TResult> SqlQuery<TResult>(string sql) where TResult : class, new();




        /// <summary>
        /// 匿名类输入sql语句查询返回一个数据集
        /// </summary>
        DataSet SqlQuery(string sql);







        /*
            /// <summary>
            /// 添加实体
            /// </summary>
            /// <param name="entity">实体对象</param>
            /// <returns>受影响行数</returns>
            Task<int> InsertAsync(T entity);

            /// <summary>
            /// 批量添加实体
            /// </summary>
            /// <param name="entities">实体列表</param>
            /// <returns>受影响行数</returns>
            Task<int> InsertRangeAsync(List<T> entities);

            /// <summary>
            /// 更新实体
            /// </summary>
            /// <param name="entity">实体对象</param>
            /// <returns>受影响行数</returns>
            Task<int> UpdateAsync(T entity);

            /// <summary>
            /// 删除实体
            /// </summary>
            /// <param name="id">主键ID</param>
            /// <returns>受影响行数</returns>
            Task<int> DeleteAsync(int id);

            /// <summary>
            /// 批量删除实体
            /// </summary>
            /// <param name="ids">主键ID列表</param>
            /// <returns>受影响行数</returns>
            Task<int> DeleteRangeAsync(List<int> ids);

            */



        #region  异步  Method


        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>实体对象</returns>
        Task<T> GetByIdAsync<T>(int id) where T : class, new();

        /// <summary>
        /// 获取所有实体列表
        /// </summary>
        /// <returns>实体列表</returns>
        Task<List<T>> GetAllAsync<T>() where T : class, new();

        /// <summary>
        /// 根据条件查询实体列表
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>符合条件的实体列表</returns>
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> whereLambda) where T : class, new();


        /// <summary>
        /// 分页查询
        /// </summary>
        Task<List<T>> GetPageListAsync<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda) where T : class, new();



        #endregion

    }
}
