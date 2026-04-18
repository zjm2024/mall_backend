using Dm.util;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopmallService.Interfaces;
using SqlSugar;
using System.Text;

namespace shopmallService.Services
{
    public class DataDictService : BaseService, IDataDictService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        private readonly IRedisQueueService _redisService;
        public DataDictService(SqlSugarHelper dbHelper,ISqlSugarClient db,IRedisQueueService redisService) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
            _redisService = redisService;
        }

        public async Task<ResultObject> getDataDictByCode(string code)
        {
            try
            {
                //先取缓存没有则取数据库
                var key = code;
                var value=await _redisService.GetStringAsync(key);

                if (value == null)  //测试 从数据库中读取
                {
                    var objout = _db.Queryable<DataDicts>().Where(it => it.Code == code).Select(it=> new { it.Value }).ToList();
                    if (objout.Count > 0)
                        value = objout[0].Value;
                    else
                        value = "";

                }

       
                
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = value };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }


    }
}
