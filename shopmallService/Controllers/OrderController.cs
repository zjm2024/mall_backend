using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using shopmallService.Interfaces;
using shopmallService.Services;
using System.Text.Json;

namespace shopmallService.Controllers
{
    [ApiController]
    [Route("shopmallApi/Order/[action]")]
    public class OrderController : ControllerBase
    {
   
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderservice;
        public OrderController(ILogger<OrderController> logger, IOrderService orderservice)
        {
            _logger = logger;
            _orderservice = orderservice;
        }

        [HttpGet]
        public ResultObject TestGet()
        {
            string aaa = "sf";
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = aaa, Count = 1, Subsidiary = 1 };

        }

        [HttpPost]
        public ResultObject TestPost([FromBody] JsonElement formData, string token)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("prod", out jValue)) ? "" : jValue.GetRawText());
            var prodentity = JsonConvert.DeserializeObject(json, typeof(products));

            string aaa = "sf";
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = prodentity, Count = 1, Subsidiary = 1 };

        }

   
    }
}
