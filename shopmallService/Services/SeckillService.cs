using Dm.util;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using publicClassLibrary.Consts;
using shopmallService.Interfaces;
using SqlSugar;
using StackExchange.Redis;
using System.Text;

namespace shopmallService.Services
{
    public class SeckillService : BaseService, ISeckillService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        private readonly IRedisQueueService _redisQueueService;
        public SeckillService(SqlSugarHelper dbHelper, ISqlSugarClient db, IRedisQueueService redisQueueService) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
            _redisQueueService = redisQueueService;
        }

        public ResultObject updateSeckillStatus(SeckillActivities sV0, string[] updateColums = null)
        {
            //判断秒杀校验
            var seckillId = sV0.SeckillId;
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;

                sV0.UpdateTime = DateTime.Now;

                Array.Resize(ref updateColums, updateColums.Length + 1);
                updateColums[updateColums.Length - 1] = "updateTime";

                bool isSuccess = Update<SeckillActivities>(sV0, updateColums);

                if (isSuccess)
                    resultobj = new ResultObject() { Flag = 1, Message = "修改状态成功!", Result = sV0 };
                else
                    resultobj = new ResultObject() { Flag = 0, Message = "修改状态成功!", Result = null };

                _db.Ado.CommitTran();

                return resultobj;
            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }


        private  List<SeckillActivities> getSeckillActiveData(int businessId,DateTime seckillTime)
        {
            var outobj = _db.Queryable<SeckillActivities>().Where(it =>it.BusinessId== businessId && SqlFunc.ToDateShort(it.StartTime) == SqlFunc.ToDateShort(seckillTime)).OrderBy(it=>it.BusinessId).ToList();
            return outobj;
   
        }



        /// <summary>
        /// 秒杀前预热：将商品数据写入 Redis
        /// </summary>
        public async Task saveHotSeckillProductsAsync(int businessId,DateTime seckillTime)
        {
            var seckillKey = $"businessid:{businessId}:seckill:{seckillTime:yyyyMMddHHmm}";

            // 1. 从数据库查询该时刻的秒杀活动商品
            var seckills =  getSeckillActiveData(businessId,seckillTime);

            // 2. 使用 Redis Pipeline 批量写入，减少网络往返
             await  _redisQueueService.CreateBatchSeckillActivityAsync(seckillKey,seckills);

          
        }

        /// <summary>
        /// 秒杀将商品数据从 Redis 读取
        /// </summary>
        public async Task getHotSeckillProductsAsync(int businessId, DateTime seckillTime)
        {
            var seckillKey = $"businessid:{businessId}:seckill:{seckillTime:yyyyMMddHHmm}";
            var seckills = await _redisQueueService.LoadBatchSeckillActivityAsync(seckillKey);
        }

        //去重保存所有秒杀时间点
        public async Task<bool> saveAllSeckillTimesAsync()
        {
            try
            {
                int pageSize = 1000;
                var list = await _redisQueueService.ScanKeysAsync(CacheConst.KeySeckillTimesPattern, pageSize);
                var values = new List<string>();
                foreach (var key in list)
                {
                    var json= await _redisQueueService.GetStringAsync(key);
                    var timesList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SeckillTimers>>(json);

                    foreach (var timer in timesList)
                    {
                        var seckillTime = timer.SeckillTime;
                        var obj = values.Find(it => it == seckillTime);
                        if (obj == null)
                        {
                            values.Add(seckillTime);
                        }
                    }
          
                }

                if (values.Count > 0)
                {
                    RedisValue[] redisValues = values.Select(v => (RedisValue)v).ToArray();
                    string json = System.Text.Json.JsonSerializer.Serialize(values);
                    await _redisQueueService.SetStringAsync(CacheConst.KeySeckillTimes, json);
                    return true;

                }
                else
                {
                    // 删除旧数据
                    await _redisQueueService.DelKeyAsync(CacheConst.KeySeckillTimes);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }

}
