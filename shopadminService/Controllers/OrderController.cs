using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using shopadminService.Interfaces;
using shopadminService.Services;
using System.Text.Json;

namespace shopadminService.Controllers
{
    [ApiController]
    [Route("shopadminApi/Order/[action]")]
    public class OrderController : ControllerBase
    {
   
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderservice;
        public OrderController(ILogger<OrderController> logger, IOrderService orderservice)
        {
            _logger = logger;
            _orderservice = orderservice;
        }


  
        [HttpPost]
        public ResultObject getOrdersPageList([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("params", out jValue)) ? "" : jValue.GetRawText());
            JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);

            int pageIndex = Convert.ToInt32(jsonElement.GetProperty("pageIndex").ToString());
            int pageSize = Convert.ToInt32(jsonElement.GetProperty("pageSize").ToString());
            int appType = Convert.ToInt32(jsonElement.GetProperty("appType").ToString());
            JsonElement outjValue;
            int? orderStatus = ((!jsonElement.TryGetProperty("orderStatus", out outjValue)) ? null : outjValue.GetInt32());
            string? searchKey = ((!jsonElement.TryGetProperty("searchKey", out outjValue)) ? null : outjValue.GetString());

            int totalCount = 0;
            var outobj = _orderservice.getOrdersPageList(pageIndex, pageSize, appType, searchKey, orderStatus, out totalCount);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount, Subsidiary = 1 };

        }

   
    }
}
