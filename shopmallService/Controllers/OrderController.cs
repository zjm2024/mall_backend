using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Models;
using shopmallService.Interfaces;
using shopmallService.Services;

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


    }
}
