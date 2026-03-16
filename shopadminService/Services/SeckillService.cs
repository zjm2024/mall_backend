using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
using System.Text;

namespace shopadminService.Services
{
    public  class SeckillService : BaseService, ISeckillService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public SeckillService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
        }

        public ResultObject getSeckillPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status)
        {
            try
            {
                //加排序
                List<OrderByModel> orderbyList = OrderByModel.Create(
                   new OrderByModel() { FieldName = "s.CreateTime", OrderByType = OrderByType.Desc });

                //加查询条件
                var conModels = new List<IConditionalModel>();
                conModels.Add(new ConditionalModel { FieldName = "s.AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });

                if (status != null)
                {
                    conModels.add(new ConditionalModel { FieldName = "s.Status", ConditionalType = ConditionalType.Equal, FieldValue = status.toString() });
                }

                if (searchKey != null)
                {
                    conModels.Add(new ConditionalCollections()
                    {
                        ConditionalList = new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>()
                    {

                       new KeyValuePair<WhereType, ConditionalModel>(
                       WhereType.And,
                       new ConditionalModel(){FieldName ="p.ProductNo",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName ="p.ProductName",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),


                    }
                    });
                }

                var totalCount=0;




                var outobj = _db.Queryable<SeckillActivities>()
                    .LeftJoin<Business>((s, b) => s.BusinessId == b.BusinessId)
                    .LeftJoin<Products>((s, b, p) => s.ProductId == p.ProductId)
                    .Where(conModels)
                    .OrderBy(orderbyList)
                   .Select((s, b, p) => new SeckillActivities
                   {
                       SeckillId = s.SeckillId,
                       AppType = s.AppType,
                       BusinessId = s.BusinessId,
                       ProductId = s.ProductId,
                       SeckillPrice = s.SeckillPrice,
                       DiscountRate = s.DiscountRate,
                       ActivityStock = s.ActivityStock,
                       UsedStock = s.UsedStock,
                       SoldPercent = s.SoldPercent,
                       ActivityDate = s.ActivityDate,
                       SeckillTime = s.SeckillTime,
                       StartTime = s.StartTime,
                       EndTime = s.EndTime,
                       Status = s.Status,
                       PerPersonLimit = s.PerPersonLimit,
                       AutoExtend = s.AutoExtend,
                       CreateTime = s.CreateTime,
                       UpdateTime = s.UpdateTime,

                       BusinessName = b.BusinessName,
                       ProductNo = p.ProductNo,
                       ProductName = p.ProductName,
                       ProductImage = p.ProductImage
                   })
                   .ToPageList(pageIndex, pageSize, ref totalCount);

            







 
              //  var outobj = GetPageList<SeckillActivities>(pageIndex, pageSize, out totalCount, conModels, orderbyList);


                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public ResultObject updateSeckill(SeckillActivities sV0, string[] updateColums = null)
        {
            //判断秒杀校验
            var seckillId = sV0.SeckillId;
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;



                if (seckillId == 0)
                {
                    int id = Add<SeckillActivities>(sV0);
                    sV0.SeckillId = id;
                    if (id > 0)
                        resultobj= new ResultObject() { Flag = 1, Message = "添加成功!", Result = sV0 };
                    else
                        resultobj= new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

                }
                else
                {
                    sV0.UpdateTime = DateTime.Now;

                    Array.Resize(ref updateColums, updateColums.Length + 1);
                    updateColums[updateColums.Length - 1] = "updateTime";

                    bool isSuccess = Update<SeckillActivities>(sV0, updateColums);
                    if (isSuccess)
                        resultobj= new ResultObject() { Flag = 1, Message = "更新成功!", Result = sV0 };
                    else
                        resultobj= new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                }

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

        public ResultObject getSeckillById(int id)
        {
            try
            {
                var outobj = GetById<SeckillActivities>(id);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public ResultObject getTimeOptions(int appType)
        {
            try
            {
                var outobj = GetList<SeckillTimers,dynamic>(it=>it.AppType== appType, it => new { it.TimerId,it.SeckillTime,it.SeckillMinutes,it.SortOrder }).OrderBy(it=>it.SortOrder);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

    }
}
