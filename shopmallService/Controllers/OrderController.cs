using Microsoft.AspNetCore.Mvc;
using NetTaste;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopmallService.Interfaces;
using System.Text.Json;

namespace shopmallService.Controllers
{
    [ApiController]
    [Route("shopmallApi/Order/[action]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderservice;
        private readonly IRedisQueueService _redisQueueService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderservice, IRedisQueueService redisQueueService)
        {
            _logger = logger;
            _orderservice = orderservice;
            _redisQueueService = redisQueueService;
        }

        [HttpGet, Anonymous]
        public ResultObject getReceiverAddress(int personalId, int appType)
        {
            var resultObject = _orderservice.getReceiverAddress(personalId, appType);
            return resultObject;
        }


        [HttpPost]
        public ResultObject saveReceiverAddress([FromBody] JsonElement formData, string token)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(Address));
            if (entity == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            var resultObject = _orderservice.saveReceiverAddress((Address)entity);
            return resultObject;
        }

        [HttpGet]
        public ResultObject delReceiverAddress(int id, string token)
        {
            return _orderservice.delReceiverAddress(id);
        }


        /// <summary>
        /// 查询订单
        /// </summary>
        [HttpGet]
        public ResultObject getOrders(int personalId, int appType, string token, string? key)
        {
            var resultObject = _orderservice.getOrders(personalId, appType, key);
            return resultObject;
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getOrdersById(int id, int appType, string token)
        {
            var resultObject = _orderservice.getOrdersById(id, appType);
            return resultObject;
        }



        /// <summary>
        /// 增加订单加Redis队列
        /// </summary>
        [HttpPost]
        public async Task<ResultObject> addOrders([FromBody] JsonElement formData,string token)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(Orders));
            if (entity == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            //生成订单号
            Random ran = new Random();
            var ord = (Orders)entity;
            ord.OrderNo= "OD" +DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);
            // 将订单发布到Redis消息队列
            var isPublish = await _redisQueueService.PublishOrderAsync(ord);
            if (isPublish)
            {
                return new ResultObject() { Flag = 1, Message = "订单排队成功!", Result = null };
            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "订单排队失败!", Result = null };
            }

        }


        /// <summary>
        /// 更新订单
        /// </summary>
        [HttpPost]
        public ResultObject updateOrders([FromBody] JsonElement formData,string token)
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
    }
}
