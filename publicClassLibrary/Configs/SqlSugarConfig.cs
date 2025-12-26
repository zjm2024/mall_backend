using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using publicClassLibrary.Models;
using publicClassLibrary.Helpers;
using SqlSugar;


namespace publicClassLibrary.Configs
{
    public static class SqlSugarConfig
    {
        //注册数据库
        public static void AddSharedDb(this IServiceCollection services, IHostApplicationBuilder builder)
        {
            //1.从配置文件里读取连接字符串
            var dbconfig = builder.Configuration.GetSection("defaultConnectionString");
            services.Configure<SqlSugarModel>(dbconfig);



            // 2.注册数据库上下文
            services.AddSingleton<SqlSugarDbContext>();
            services.AddSingleton<ISqlSugarClient>(provider =>
            {
                var context = provider.GetRequiredService<SqlSugarDbContext>();
                return context.GetDbClient();
            });

            // 3.注册数据库基本操作
            services.AddSingleton<SqlSugarHelper>();


        }


    }

    public class SqlSugarDbContext : IDisposable
    {
        private readonly IOptions<SqlSugarModel> _options;
        private SqlSugarClient _db;
        public SqlSugarDbContext(IOptions<SqlSugarModel> options)
        {
 
            _options = options;
            InitializeDb();
        }
        private void InitializeDb()
        {
            var sqlSugarModel = _options.Value;
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = sqlSugarModel.Connection,
                DbType = sqlSugarModel.DbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
        }
        public ISqlSugarClient GetDbClient()
        {
   
            return _db;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db?.Dispose();
                _db = null;
            }
        }
    }
}
