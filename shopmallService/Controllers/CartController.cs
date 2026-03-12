using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
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

        [HttpGet, Anonymous]
        public ResultObject getCartByIndex(int personalId, int appType)
        {
            var resultObject = _cartservice.getCartByIndex(personalId, appType);
            return resultObject;

        }


    }
}
