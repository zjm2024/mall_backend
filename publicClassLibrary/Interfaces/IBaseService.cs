using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace publicClassLibrary.Interfaces
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
        /// <param name="orderbyList">排序表达式</param>
        /// <returns>符合条件的实体列表</returns>
        List<T> GetList<T>(Expression<Func<T, bool>> whereLambda, List<OrderByModel> orderbyList = null) where T : class, new();


        /// <summary>
        /// 条件查询+排序
        /// </summary>
        /// <param name="conModels">查询条件表达式</param>
        /// <param name="orderbyList">排序表达式</param>
        /// <returns>符合条件的实体列表</returns>
        List<T> GetList<T>(List<IConditionalModel> conModels, List<OrderByModel> orderbyList = null) where T : class, new();




        /// <summary>
        /// 匿名类条件查询+排序
        /// </summary>
        List<TResult> GetList<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new();


        List<TResult> GetList<T, TResult>(List<IConditionalModel> conModels, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new();




        /// <summary>
        /// 分页查询+排序
        /// </summary>
        List<T> GetPageList<T>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, List<OrderByModel> orderbyList = null) where T : class, new();


        List<T> GetPageList<T>(int pageIndex, int pageSize, out int totalCount, List<IConditionalModel> conModels, List<OrderByModel> orderbyList = null) where T : class, new();

        /// <summary>
        /// 匿名类分页查询+排序
        /// </summary>
        List<TResult> GetPageList<T, TResult>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new();


        List<TResult> GetPageList<T, TResult>(int pageIndex, int pageSize, out int totalCount, List<IConditionalModel> conModels, Expression<Func<T, TResult>> selectExpression, List<OrderByModel> orderbyList = null) where T : class, new() where TResult : class, new();



        /// <summary>
        /// 树状查询
        /// </summary>
        List<T> GetTreeList<T>(Expression<Func<T, IEnumerable<object>>> childListExpression, Expression<Func<T, object>> parentIdExpression, object rootValue, Expression<Func<T, object>> primaryKeyExpression) where T : class, new();
 








        /// <summary>
        /// 匿名类输入sql语句查询返回一个数据表
        /// </summary>
        List<TResult> SqlQuery<TResult>(string sql) where TResult : class, new();




        /// <summary>
        /// 匿名类输入sql语句查询返回一个数据集
        /// </summary>
        DataSet SqlQuery(string sql);

        /// <summary>
        ///  匿名类求和和求记录数List<GroupByModel>
        /// </summary>

        List<TResult> GetSum<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, List<GroupByModel> groupbyList = null) where T : class, new() where TResult : class, new();


        /// <summary>
        /// 匿名类求和和求记录数用Expression<Func<T, object>>
        /// </summary>

        List<TResult> GetSum<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression, Expression<Func<T, object>> groupbyExpression) where T : class, new() where TResult : class, new();


        /// <summary>
        /// 是否存在符合条件的记录
        /// </summary>
        bool RecordExist<T, TResult>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TResult>> selectExpression) where T : class, new() where TResult : class, new();

        int Add<T>(T entity) where T : class, new();



        bool Delete<T>(Expression<Func<T, bool>> whereLambda) where T : class, new();

        bool Delete<T>(int id) where T : class, new();

        bool Delete<T>(List<int> ids) where T : class, new();


        bool Update<T>(T entity, Expression<Func<T, object>> updateColumnsExpression) where T : class, new();

        bool Update<T>(T entity, string[] updateColumns = null) where T : class, new();


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
