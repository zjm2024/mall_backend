using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopadminService.Interfaces;
using System.Text.Json;

namespace shopadminService.Controllers
{
    [Anonymous]
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

            var outobj = _orderservice.getOrdersPageList(pageIndex, pageSize, appType, searchKey, orderStatus);
            return outobj;

        }



        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getOrdersById(int id)
        {
            var outobj = _orderservice.getOrdersById(id);
            return outobj;
        }


        /// <summary>
        /// 更新订单
        /// </summary>
        [HttpPost]
        public ResultObject updateOrders([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(Orders));
            if (entity == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            //获取json中的修改字段
            List<string> listColums = new List<string>();

            JObject jsonobj = JObject.Parse(json);
            foreach (JProperty prop in jsonobj.Properties())
            {
                listColums.Add(prop.Name);

            }
            string[] updateColums = listColums.ToArray();



            return _orderservice.updateOrders((Orders)entity, updateColums);
        }



        /// <summary>
        /// 查询子订单
        /// </summary>
        [HttpPost]
        public ResultObject getOrdersSubsPageList([FromBody] JsonElement formData)
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

  
            var outobj = _orderservice.getOrdersSubsPageList(pageIndex, pageSize, appType, searchKey, orderStatus);
            return outobj;
        }

        [HttpGet]
        public ResultObject getOrdersSubsById(int id)
        {
            var outobj = _orderservice.getOrdersSubsById(id);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = 1, Subsidiary = 1 };
        }

    }
}
