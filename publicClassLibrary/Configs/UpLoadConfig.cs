using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Models;
using SqlSugar;
using System;
using System.Net.Http;


namespace publicClassLibrary.Configs
{
    public static class UpLoadConfig
    {

        public static void AddSharedUpLoad(this IServiceCollection services, IHostApplicationBuilder builder)
        {

            services.AddScoped<IUploadConfig>(sp =>
            {

                string GetDefaultSavePath(IHttpContextAccessor _httpContextAccesso)
                {
                    var httpContext = _httpContextAccesso.HttpContext;
                    // 获取 wwwroot 目录（Web 根目录）
                    var webRootPath = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootPath;
                    return webRootPath;
                }

                string GetDefaultUrl(IHttpContextAccessor _httpContextAccesso)
                {
                    var request = _httpContextAccesso.HttpContext?.Request;
                    string url = request.Scheme + "://" + request.Host.ToString();
                    return url;
                }

                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configuration = sp.GetRequiredService<IConfiguration>();


                var configUploadPath = configuration["defaultUploadSetings:UploadPath"];
                var configUploadUrl = configuration["defaultUploadSetings:UploadUrl"];


                // 配置文件有值则使用配置值，无值则从 HttpContext 获取
                var savePath = string.IsNullOrEmpty(configUploadPath)
                    ? GetDefaultSavePath(httpContextAccessor)
                    : configUploadPath;

                var baseUrl = string.IsNullOrEmpty(configUploadUrl)
                    ? GetDefaultUrl(httpContextAccessor)
                    : configUploadUrl;

                // 确保目录存在
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                return new UpLoadModel
                {
                    UploadPath = savePath,
                    UploadUrl = baseUrl
                };

            });

        }

   
    }

}
