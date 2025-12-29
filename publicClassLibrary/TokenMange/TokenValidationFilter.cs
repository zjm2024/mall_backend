using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using publicClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.TokenMange
{
    /// <summary>
    /// Token验证过滤器（替代旧版OnActionExecuting）
    /// </summary>
    public class TokenValidationFilter : IAsyncActionFilter
    {
        private const string UserToken = "token";

        /// <summary>
        /// 异步Action执行前拦截（对应旧版OnActionExecuting）
        /// </summary>
        /// <param name="context">Action执行上下文（替代HttpActionContext）</param>
        /// <param name="next">执行下一个过滤器/Action</param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. 判断是否标记了匿名访问特性（优先方法，其次控制器）
            var isAnonymous = context.ActionDescriptor.EndpointMetadata.Any(m => m is AnonymousAttribute)
                              || context.Controller.GetType().GetCustomAttributes(typeof(AnonymousAttribute), false).Any();

            if (!isAnonymous)
            {
                // 2. 未标记匿名特性，执行Token验证
                var token = TokenVerification(context);

                // 如果Token验证失败，直接返回结果，终止请求
                if (token == null)
                {
                    context.Result = new UnauthorizedObjectResult(
                     new ResultObject() { Flag = 401, Message = "Token无效或已过期", Result = null }
                    );

                    return; // 终止后续执行
                }
            }

            // 3. 执行后续过滤器/Action逻辑
            await next();
        }

        /// <summary>
        /// Token验证逻辑（适配.NET Core 8.0上下文）
        /// </summary>
        /// <param name="context">Action执行上下文</param>
        /// <returns>验证通过返回Token字符串，失败返回null</returns>
        private string? TokenVerification(ActionExecutingContext context)
        {
            // ===== 核心：从请求中获取Token（根据你的业务调整）=====
            // 方式1：从Header中获取（推荐）
            var token = "";
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                //如果没有请求头 则从传递过来的token参数中获取       
                 token = GetToken(context.HttpContext);
    
            }
            else
                 token = authHeader.ToString().Replace("Bearer ", "").Trim();




            // 1. 验证Token是否为空
            if (string.IsNullOrEmpty(token)) return null;

            // 2. 验证Token有效性（如JWT解密、Redis查询等）
            // 示例：替换为你的真实验证逻辑
            bool isValid = ValidateToken(token);

            return isValid ? token : null;
        }

        /// <summary>
        /// 模拟Token验证逻辑（替换为你的真实业务）
        /// </summary>
        /// <param name="token">待验证的Token</param>
        /// <returns>是否有效</returns>
        private bool ValidateToken(string token)
        {
            // 这里写你的真实验证逻辑：
            // - JWT验证（校验签名、过期时间）
            // - Redis查询Token是否存在
            // - 数据库查询Token有效性



            // 判断token是否有效
            /*
            if (!CacheManager.TokenIsExist(token))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new ResultObject() { Flag = 401, Message = "Token 失效", Result = null });
                return "";
                //throw new Exception("Token已失效，请重新登陆!");
            }

            */


            return !string.IsNullOrEmpty(token) && token.Length > 10; // 示例逻辑
        }

        /// <summary>
        /// Token验证逻辑（从传递过来的token参数中获取token）
        /// </summary>
        private string GetToken(HttpContext context )
        {
            string httpMethod = context.Request.Method;
            var token = "";
            if (httpMethod == "POST")
            {
                if (context.Request.Query.TryGetValue(UserToken, out var value))
                {
                    token = value.ToString();
                }


            }
            else if (httpMethod == "GET")
            {
                // 按参数名获取值
                if  (context.Request.Query.TryGetValue(UserToken, out var value))
                {
                    token = value.ToString();
                }

          
            }
            else
            {
                //throw new Exception("暂未开放其它访问方式!");
            }

            return token;
        }
    }
}
