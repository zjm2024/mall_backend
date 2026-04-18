using Dm.util;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using publicClassLibrary.Models;
using SqlSugar;
using StackExchange.Redis;

namespace publicClassLibrary.Configs
{
    public static class RedisConfig
    {
        //注册Redis服务
        public static void AddSharedRedis(this IServiceCollection services, IHostApplicationBuilder builder)
        {


            //1.从配置文件里读取连接字符串
            var config = builder.Configuration.GetSection("defaultRedis");

            // 2.注册Redis上下文
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var constring = config.Value.toString();
                return ConnectionMultiplexer.Connect(constring);
            });


   
        }


    }
}
