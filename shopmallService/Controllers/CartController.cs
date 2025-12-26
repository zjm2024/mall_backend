using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Models;
using shopmallService.Interfaces;
using shopmallService.Services;

namespace shopmallService.Controllers
{
    [ApiController]
    [Route("shopmallApi/Cart/[action]")]
    public class CartController : ControllerBase
    {
   
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _cartservice;
        public CartController(ILogger<CartController> logger, ICartService cartservice)
        {
            _logger = logger;
            _cartservice = cartservice;
        }

        [HttpGet]
        public ResultObject TestGet()
        {
            string aaa = "";
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = aaa, Count = 1, Subsidiary = 1 };

        }


    }
}
