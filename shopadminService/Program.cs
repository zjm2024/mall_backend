using publicClassLibrary.Configs;
using shopadminService.Interfaces;
using shopadminService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSharedSwagger(); // 1.使用Swagger共享配置

builder.Services.AddSharedDb(builder);    // 2.使用SqlSugar共享配置

builder.Services.AddSharedCors(); //3.配置跨域请求

// 4.注册服务
builder.Services.AddScoped<IUserService, UserService>(); //用户服务
builder.Services.AddScoped<ICategoryService, CategoryService>(); //类型服务
builder.Services.AddScoped<IProductService, ProductService>(); //商品服务

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSharedSwagger(app.Environment); // 配置Swagger HTTP请求管道

app.UseSharedCors();  // 配置 跨域请求

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
