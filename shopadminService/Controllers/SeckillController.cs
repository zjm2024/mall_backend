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
    [Route("shopadminApi/Seckill/[action]")]

    public class SeckillController : ControllerBase
    {
        private readonly ILogger<SeckillController> _logger;
        private readonly ISeckillService _seckillservice;
        public SeckillController(ILogger<SeckillController> logger, ISeckillService seckillservice)
        {
            _logger = logger;
            _seckillservice = seckillservice;
        }

        [HttpPost]
        public ResultObject getSeckillPageList([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("params", out jValue)) ? "" : jValue.GetRawText());
            JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);

            int pageIndex = Convert.ToInt32(jsonElement.GetProperty("pageIndex").ToString());
            int pageSize = Convert.ToInt32(jsonElement.GetProperty("pageSize").ToString());
            int appType = Convert.ToInt32(jsonElement.GetProperty("appType").ToString());
            int businessId = Convert.ToInt32(jsonElement.GetProperty("businessId").ToString());

            JsonElement outjValue;
            int? status = ((!jsonElement.TryGetProperty("status", out outjValue)) ? null : outjValue.GetInt32());
            string? searchKey = ((!jsonElement.TryGetProperty("searchKey", out outjValue)) ? null : outjValue.GetString());

            var outobj = _seckillservice.getSeckillPageList(pageIndex, pageSize, appType, businessId,searchKey, status);
            return outobj;

        }



        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getSeckillById(int id)
        {
            var outobj = _seckillservice.getSeckillById(id);
            return outobj;
        }


        /// <summary>
        /// 更新秒杀
        /// </summary>
        [HttpPost]
        public ResultObject updateSeckill([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(SeckillActivities));
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



            return _seckillservice.updateSeckill((SeckillActivities)entity, updateColums);
        }

        /// <summary>
        /// 审核秒杀
        /// </summary>
        [HttpPost]
        public async Task<ResultObject> checkSeckill([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(SeckillActivities));
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



             return await _seckillservice.checkSeckill((SeckillActivities)entity, updateColums);
        }

        [HttpGet]
        public ResultObject getTimeOptions(int appType)
        {
            var outobj = _seckillservice.getTimeOptions(appType);
            return outobj;
        }
    }
    
}
