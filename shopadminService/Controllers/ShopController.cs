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
    [Route("shopadminApi/Shop/[action]")]



    public class ShopController : ControllerBase
    {

        private readonly ILogger<ShopController> _logger;
        private readonly IShopService _shopservice;
        public ShopController(ILogger<ShopController> logger, IShopService shopservice)
        {
            _logger = logger;
            _shopservice = shopservice;
        }



        [HttpPost]
        public ResultObject getShopsPageList([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("params", out jValue)) ? "" : jValue.GetRawText());
            JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);

            int pageIndex = Convert.ToInt32(jsonElement.GetProperty("pageIndex").ToString());
            int pageSize = Convert.ToInt32(jsonElement.GetProperty("pageSize").ToString());
            int appType = Convert.ToInt32(jsonElement.GetProperty("appType").ToString());
            JsonElement outjValue;
            int? status = ((!jsonElement.TryGetProperty("status", out outjValue)) ? null : outjValue.GetInt32());
            string? searchKey = ((!jsonElement.TryGetProperty("searchKey", out outjValue)) ? null : outjValue.GetString());

            var outobj = _shopservice.getShopsPageList(pageIndex, pageSize, appType, searchKey, status);
            return outobj;

        }



        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getShopsById(int id)
        {
            var outobj = _shopservice.getShopsById(id);
            return outobj;
        }


        /// <summary>
        /// 更新商户
        /// </summary>
        [HttpPost]
        public ResultObject updateShops([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(Business));
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



            return _shopservice.updateShops((Business)entity, updateColums);
        }


        /// <summary>
        /// 删除商户
        /// </summary>
        [HttpGet]
        public ResultObject deleteShops(int id)
        {
            return _shopservice.deleteShops(id);
        }


    }


    
}
