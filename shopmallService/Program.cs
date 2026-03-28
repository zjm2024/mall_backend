using Newtonsoft.Json.Serialization;
using publicClassLibrary.Configs;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Services;
using publicClassLibrary.TokenMange;
using shopmallService.Hubs;
using shopmallService.Interfaces;
using shopmallService.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSharedSwagger(); // 1.使用Swagger共享配置

builder.Services.AddSharedDb(builder);    // 2.使用SqlSugar共享配置

builder.Services.AddSharedCors(); //3.配置跨域请求

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false")
);


// 注册Redis消息队列服务
builder.Services.AddSingleton<IRedisQueueService, RedisQueueService>();

// 4.注册服务
builder.Services.AddScoped<ITokenService, TokenService>(); //Token服务
builder.Services.AddScoped<IProductService, ProductService>(); //商品服务
builder.Services.AddScoped<ICartService, CartService>(); //购物车服务
builder.Services.AddScoped<IOrderService, OrderService>(); //订单服务
builder.Services.AddScoped<ISeckillService, SeckillService>(); //秒杀服务
builder.Services.AddScoped<IDataDictService, DataDictService>(); //数据字典服务



// 注册后台服务处理订单消息
builder.Services.AddHostedService<OrderBackgroundService>();

// 注册后台服务处理秒杀消息
builder.Services.AddHostedService<SeckillBackgroundService>();
// 注册后台定时发送消息
builder.Services.AddHostedService<QuartzHostedService>();

builder.Services.AddSingleton<ChatJob>();


builder.Services.AddSingleton<ChatJobFactory>();


// 添加SignalR服务
builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = System.TimeSpan.FromSeconds(5);
});


// 注册自定义转换器
builder.Services.AddControllers(options =>
{
    // 注册全局Token验证过滤器
    options.Filters.Add<TokenValidationFilter>();
})
        .AddJsonOptions(options =>
        {
            //保持字段名大小写
            options.JsonSerializerOptions.PropertyNamingPolicy = null;

            options.JsonSerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter()); // 如需要
            options.JsonSerializerOptions.Converters.Add(
                new publicClassLibrary.Helpers.DateTimeConverter("yyyy-MM-dd HH:mm:ss"));          // 关键
        });


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSharedSwagger(app.Environment); // 配置Swagger HTTP请求管道

app.UseSharedCors();  // 配置 跨域请求


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWebSockets();//webstock 关键

app.UseMiddleware<shopmallService.Services.WebSocketMiddleware>();

app.MapControllers();
/*

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<OrderNotificationHub>("/ordernotificationhub");
    endpoints.MapControllers();
});
*/
app.MapGet("/", () => "Hello World!");

app.Run();
