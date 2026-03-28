using Dm.util;
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
    public class SeckillService : BaseService, ISeckillService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public SeckillService(SqlSugarHelper dbHelper,ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
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

    }
}
