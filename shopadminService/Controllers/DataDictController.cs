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
    [Route("shopadminApi/DataDict/[action]")]



    public class DataDictController : ControllerBase
    {

        private readonly ILogger<DataDictController> _logger;
        private readonly IDataDictService _datadictservice;
        public DataDictController(ILogger<DataDictController> logger, IDataDictService datadictservice)
        {
            _logger = logger;
            _datadictservice = datadictservice;
        }



        [HttpPost]
        public ResultObject getDataDictPageList([FromBody] JsonElement formData)
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

            var outobj = _datadictservice.getDataDictPageList(pageIndex, pageSize, appType, searchKey, status);
            return outobj;

        }



        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getDataDictById(int id)
        {
            var outobj = _datadictservice.getDataDictById(id);
            return outobj;
        }



        /// <summary>
        /// 更新数据字典
        /// </summary>
        [HttpPost]
        public async Task<ResultObject> updateDataDict([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(DataDicts));
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



            return await _datadictservice.updateDataDict((DataDicts)entity, updateColums);
        }


        /// <summary>
        /// 删除数据字典
        /// </summary>
        [HttpGet]
        public Task<ResultObject> deleteDataDict(int id)
        {
            return _datadictservice.deleteDataDict(id);
        }


    }


    
}
