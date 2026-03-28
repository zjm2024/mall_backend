using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopmallService.Interfaces;
using shopmallService.Services;
using System.Linq.Expressions;
using System.Text.Json;

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
        [HttpGet, Anonymous]
        public ResultObject getInfoList(int businessId,int appType)
        {
        
            var resultObject = _productservice.getInfoList(businessId, appType);
            return resultObject;
        }


        /// <summary>
        /// 根据pageIndex,pageSize 分页获取实体，自动带输出参数返回总记录
        /// </summary>
        [HttpGet, Anonymous]
        public ResultObject getProductsPageList(int pageIndex, int pageSize, string treePath, string? searchkey, int appType)
        {
            var resultObject = _productservice.getProductsPageList(pageIndex, pageSize, treePath, searchkey,appType);
            return resultObject;
        }


        [HttpGet, Anonymous]
        public ResultObject getHotProductsPageList(int pageIndex, int pageSize, string? searchkey, int appType)
        {
            var resultObject = _productservice.getHotProductsPageList(pageIndex, pageSize, searchkey, appType);
            return resultObject;
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        [HttpGet, Anonymous]
        public ResultObject getProductsById(int productId)
        {
            var resultObject = _productservice.getProductsById(productId);
            return resultObject;
        }




        /// <summary>
        /// 更新或插入分类
        /// </summary>
        [HttpPost]
        public ResultObject updateCategories([FromBody] JsonElement formData, string token)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("categories", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(Categories));
            if (entity == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
           return _productservice.updateCategories((Categories)entity);
        }


        /// <summary>
        /// 获取商城首页数据
        /// </summary>
        [HttpGet, Anonymous]
        public ResultObject getCardByShare(int appType)
        {
            var resultObject = _productservice.getCardByShare(appType);
            return resultObject;
        }

        /// <summary>
        /// 获取秒杀时间列表
        /// </summary>
        [HttpGet, Anonymous]
        public ResultObject getSeckillTimersList(int appType)
        {
            var resultObject = _productservice.getSeckillTimersList(appType);
            return resultObject;
        }

        /// <summary>
        /// 获取当天秒杀的商品列表
        /// </summary>
        [HttpGet, Anonymous]
        public ResultObject getCurDateTimeSeckillList(int pageIndex, int pageSize, string timer, int appType)
        {
            
            var resultObject = _productservice.getCurDateTimeSeckillList(pageIndex, pageSize,timer, appType);
            return resultObject;
        }

  

        /*
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
