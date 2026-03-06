using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopadminService.Interfaces;
using shopadminService.Services;
using System.Text.Json;


namespace shopadminService.Controllers
{
    [Anonymous]
    [ApiController]
    [Route("shopadminApi/Product/[action]")]
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
        /// 根据pageIndex,pageSize 分页获取实体，自动带输出参数返回总记录
        /// </summary>
        [HttpPost]
        public ResultObject getProductsPageList([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("params", out jValue)) ? "" : jValue.GetRawText());
            JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);

            int pageIndex = Convert.ToInt32(jsonElement.GetProperty("pageIndex").ToString());
            int pageSize = Convert.ToInt32(jsonElement.GetProperty("pageSize").ToString());
            int appType = Convert.ToInt32(jsonElement.GetProperty("appType").ToString());
            int businessId = Convert.ToInt32(jsonElement.GetProperty("businessId").ToString());

            JsonElement outjValue;
            string? productName = ((!jsonElement.TryGetProperty("productName", out outjValue)) ? null : outjValue.ToString());

            int? productStatus = ((!jsonElement.TryGetProperty("productStatus", out outjValue)) ? null : outjValue.GetInt32());

            string? categoryIds = ((!jsonElement.TryGetProperty("categoryIds", out outjValue)) ? null : outjValue.ToString());


            int totalCount = 0;
            var outobj = _productservice.getProductsPageList(pageIndex, pageSize, appType, businessId, productName, productStatus, categoryIds, out totalCount);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount, Subsidiary = 1 };
        }




        /// <summary>
        /// 更新或插入商品
        /// </summary>
        [HttpPost]
        public ResultObject updateProducts([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            string delSpecsids = ((!formData.TryGetProperty("delSpecsids", out jValue)) ? "" : jValue.ToString());

            var entity = JsonConvert.DeserializeObject(json, typeof(Products));
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



            return _productservice.updateProducts((Products)entity, updateColums, delSpecsids);
        }


        /// <summary>
        /// 删除商品
        /// </summary>
        [HttpGet]
        public ResultObject deleteProducts(int id)
        {
            return _productservice.deleteProducts(id);
        }

        /// <summary>
        /// 批量删除商品
        /// </summary>
        [HttpGet]
        public ResultObject deleteBatchProducts(string ids)
        {
            return _productservice.deleteBatchProducts(ids);
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
        /// 修改规格图片
        /// </summary>
        [HttpPost]
        public ResultObject updateProductSpecsImage([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(ProductSpecs));
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



            return _productservice.updateProductSpecsImage((ProductSpecs)entity, updateColums);
        }




        /// <summary>
        /// 删除商品规格
        /// </summary>
        [HttpGet]
        public ResultObject deleteProductSpecs(int id)
        {
            return _productservice.deleteProductSpecs(id);
        }



        /// <summary>
        /// 批量删除商品规格
        /// </summary>
        /// <param name="ids">规格ids</param>
        [HttpGet]
        public ResultObject deleteBatchProductSpecs(string ids)
        {
            return _productservice.deleteBatchProductSpecs(ids);
        }



        /// <summary>
        /// 查询状态为显示的商品分类按树状结构输出 分类编号和分类名称，图片
        /// </summary>
        [HttpGet]
        public ResultObject getCategoriesOptions(int appType, int businessId)
        {
            var list = _productservice.getCategoriesOptions(appType, businessId);

            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = list.Count, Subsidiary = 1 };
        }

    }
}
