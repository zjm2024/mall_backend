using Newtonsoft.Json.Serialization;
using publicClassLibrary.Configs;
using publicClassLibrary.Helpers;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using shopadminService.Services;
using StackExchange.Redis;
using System.ComponentModel;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor(); // 注册 IHttpContextAccessor

builder.Services.AddSharedSwagger(); // 1.使用Swagger共享配置

builder.Services.AddSharedDb(builder);    // 2.使用SqlSugar共享配置

builder.Services.AddSharedCors(); //3.配置跨域请求

builder.Services.AddSharedUpLoad(builder); //4.配置上传文件

builder.Services.AddSharedRedis(builder); //5.使用Redis

// 注册Redis消息队列服务
builder.Services.AddSingleton<IRedisQueueService, RedisQueueService>();



// 注册服务
builder.Services.AddScoped<IUserService, UserService>(); //用户服务
builder.Services.AddScoped<ICategoryService, CategoryService>(); //类型服务
builder.Services.AddScoped<IProductService, ProductService>(); //商品服务
builder.Services.AddScoped<IOrderService, OrderService>(); //订单服务
builder.Services.AddScoped<ISeckillService, SeckillService>(); //秒杀服务
builder.Services.AddScoped<IBusinessService, BusinessService>(); //店铺服务
builder.Services.AddScoped<IPersonalService, PersonalService>(); //员工服务
builder.Services.AddScoped<IDataDictService, DataDictService>(); //数据字典服务

builder.Services.AddScoped<IUpLoadFileService, UpLoadFileService>(); //上传文件服务





// 注册自定义转换器
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter()); // 如需要
            options.JsonSerializerOptions.Converters.Add(
                new publicClassLibrary.Helpers.DateTimeConverter("yyyy-MM-dd HH:mm:ss"));          // 关键
        });


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSharedSwagger(app.Environment); // 配置Swagger HTTP请求管道

app.UseSharedCors();  // 配置 跨域请求


app.UseDefaultFiles();

app.UseStaticFiles(); // 启用静态文件访问（默认映射 wwwroot 文件夹）


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
