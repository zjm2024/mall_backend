using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using publicClassLibrary.Configs;
using publicClassLibrary.TokenMange;
using shopmallService.Interfaces;
using shopmallService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSharedSwagger(); // 1.使用Swagger共享配置

builder.Services.AddSharedDb(builder);    // 2.使用SqlSugar共享配置

// 3.注册服务
builder.Services.AddScoped<IProductService, ProductService>(); //商品服务
builder.Services.AddScoped<ICartService, CartService>(); //购物车服务
builder.Services.AddScoped<IOrderService, OrderService>(); //订单服务

//builder.Services.AddControllers();



// 4.添加控制器
builder.Services.AddControllers(options =>
{
    // 注册全局Token验证过滤器
    options.Filters.Add<TokenValidationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSharedSwagger(app.Environment); // 1.配置Swagger HTTP请求管道




app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
