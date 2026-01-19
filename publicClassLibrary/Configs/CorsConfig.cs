using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace publicClassLibrary.Configs
{
    public static class CorsConfig
    {
        //注册跨域请求服务
        public static void AddSharedCors(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                 // .WithExposedHeaders(new[] { "Location", "Upload-Offset", "Upload-Length" })
                    .AllowAnyHeader()
                    .AllowAnyMethod();


                });
            });
        }
        // 使用部分
        public static IApplicationBuilder UseSharedCors(this IApplicationBuilder app)
        {
            app.UseCors("MyCorsPolicy");
            return app;
        }
    }
}
