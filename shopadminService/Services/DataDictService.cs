using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Interfaces;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
using System.Text;
using System.Threading.Tasks;

namespace shopadminService.Services
{
    public  class DataDictService : BaseService, IDataDictService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        private readonly IRedisQueueService _redisService;
        public DataDictService(SqlSugarHelper dbHelper, ISqlSugarClient db, IRedisQueueService redisService) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
            _redisService = redisService;
        }

        public ResultObject getDataDictPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status)
        {
            try
            {
                //加排序
                List<OrderByModel> orderbyList = OrderByModel.Create(
                   new OrderByModel() { FieldName = "CreateTime", OrderByType = OrderByType.Desc });

                //加查询条件
                var conModels = new List<IConditionalModel>();
                conModels.Add(new ConditionalModel { FieldName = "AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });

                if (status != null)
                {
                    conModels.add(new ConditionalModel { FieldName = "Status", ConditionalType = ConditionalType.Equal, FieldValue = status.toString() });
                }

                if (searchKey != null)
                {
                    conModels.Add(new ConditionalCollections()
                    {
                        ConditionalList = new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>()
                    {

                       new KeyValuePair<WhereType, ConditionalModel>(
                       WhereType.And,
                       new ConditionalModel(){FieldName ="Code",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName ="Name",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),


                    }
                    });
                }

                var totalCount=0;

                var outobj = GetPageList<DataDicts, dynamic>(pageIndex, pageSize, out totalCount, conModels, it => new {
                    dataDictId=it.DataDictId,
                    code = it.Code,
                    name = it.Name,
                    value = it.Value,
                    appType = it.AppType,
                    status = it.Status,
                    createTime = SqlFunc.ToString(it.CreateTime)
                }, orderbyList).ToList();


                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public async Task<ResultObject> updateDataDict(DataDicts bV0, string[] updateColums = null)
        {
            //判断
            var dataDictId = bV0.DataDictId;
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;



                if (dataDictId == 0)
                {
                    int id = Add<DataDicts>(bV0);
                    bV0.DataDictId = id;
                    if (id > 0)
                    {
                        //缓存
                        var key = bV0.Code;
                        var value = bV0.Value;
                        await _redisService.SetStringAsync(key, value);
                        resultobj = new ResultObject() { Flag = 1, Message = "添加成功!", Result = bV0 };
                    } 
                    else
                        resultobj= new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

                }
                else
                {
                    bV0.UpdateTime = DateTime.Now;

                    Array.Resize(ref updateColums, updateColums.Length + 1);
                    updateColums[updateColums.Length - 1] = "updateTime";

                    bool isSuccess = Update<DataDicts>(bV0, updateColums);
                    if (isSuccess)
                    {
                        //缓存
                        var key = bV0.Code;
                        var value = bV0.Value;
                        await _redisService.SetStringAsync(key, value);
                        resultobj = new ResultObject() { Flag = 1, Message = "更新成功!", Result = bV0 };
                    }
                    else
                        resultobj = new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
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

        public ResultObject getDataDictById(int id)
        {
            try
            {
                var outobj = GetById<DataDicts>(id);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }



        public async Task<ResultObject> deleteDataDict(int id)
        {
            _db.Ado.BeginTran();
            try
            {
                var bV0= GetById<DataDicts>(id);
                dynamic resultobj;
                bool isSuccess = Delete<DataDicts>(id);
                if (isSuccess)
                {
                    resultobj= new ResultObject() { Flag = 1, Message = "删除成功!", Result = id };
                    //缓存
                    var key = bV0.Code;
                    await _redisService.DelKeyAsync(key);
                }
                else
                {
                    resultobj= new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }

                _db.Ado.CommitTran();
                return resultobj;

            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "删除失败!" + ex.toString(), Result = null };
            }

        }


    }
}
