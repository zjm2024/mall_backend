using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;

namespace publicClassLibrary.Configs
{
    public static class SwaggerConfig
    {
        // 服务注册部分
        public static void AddSharedSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "示例 .NET 8 Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "乐聊开发团队",
                        Email = "dev@example.com"
                    }
                });
                /*
                 * 
                // 定义安全方案（如 JWT）
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",

                };

                c.AddSecurityDefinition("Bearer", securityScheme);



                // 强制所有操作需 Bearer Token
                // 第二步：添加全局安全要求（强制所有接口携带 Token）
                c.AddSecurityRequirement(SwaggerHelper.SwaggerOpenApiSecurityFactory);

                */

            });

        }

        // 管道配置部分
        public static IApplicationBuilder UseSharedSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 配置HTTP请求管道
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProject API V1");
                });
            }

            return app;
        }

    }
}
