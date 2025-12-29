using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopmallService.Interfaces;
using shopmallService.Services;
using System.Linq.Expressions;

namespace shopmallService.Controllers
{
    [ApiController]
    [Route("shopmallApi/Product/[action]")]
    public class ProductController : ControllerBase
    {
   
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productservice;
        public ProductController(ILogger<ProductController> logger, IProductService productservice)
        {
            _logger = logger;
            _productservice = productservice;
        }


        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getCategoriesList(int appType)
        {
        
            var list = _productservice.getCategoriesList(appType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = list.Count, Subsidiary = 1 };
        }

        /*
        [HttpGet, Anonymous]
        public ResultObject getTokenAll()
        {
            var list = _productservice.getTokenAll();
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = 1, Subsidiary = 1 };
        }

        /// <summary>
        /// 查询全表实体 不推荐使用 除非表记录很少的情况下允许查询
        /// </summary>
        [HttpGet]
        public  ResultObject getProductsAll()
        {
            var list = _productservice.getProductsAll();
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = list.Count, Subsidiary = 1 };
        }
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getProductsById(int id)
        {
            var outobj = _productservice.getProductsById(id);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = 1, Subsidiary = 1 };
        }

        /// <summary>
        /// 根据pageIndex,pageSize 分页获取实体，自动带输出参数返回总记录
        /// </summary>
        [HttpGet]
        public ResultObject getProductsPageList(int pageIndex, int pageSize,int appType)
        {
            int totalCount = 0;
            var outobj = _productservice.getProductsPageList(pageIndex, pageSize, appType, out totalCount);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount, Subsidiary = 1 };
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getProductsList( int appType)
        {

            var list = _productservice.getProductsList(appType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = list.Count, Subsidiary = 1 };
        }

        [HttpGet]
        public ResultObject getCustomClumnsProductsList(int appType)
        {

            var list = _productservice.getCustomClumnsProductsList(appType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = list.Count, Subsidiary = 1 };
        }
        [HttpGet]
        public ResultObject getCustomClumnsProductsPageList(int pageIndex, int pageSize, int appType)
        {

            int totalCount = 0;
            var outobj = _productservice.getCustomClumnsProductsPageList(pageIndex, pageSize, appType, out totalCount);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount, Subsidiary = 1 };

        }



        [HttpGet]
        public ResultObject getProductSum(int appType)
        {
            var outobj = _productservice.getProductSum(appType);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = 1, Subsidiary = 1 };
        }
        */
    }
}
