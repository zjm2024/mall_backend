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
    public  class BusinessService : BaseService, IBusinessService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public BusinessService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
        }

        public ResultObject getBusinessPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status)
        {
            try
            {
                //加排序
                List<OrderByModel> orderbyList = OrderByModel.Create(
                   new OrderByModel() { FieldName = "CreatedAt", OrderByType = OrderByType.Desc });

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
                       new ConditionalModel(){FieldName ="BusinessNo",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName ="BusinessName",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),


                    }
                    });
                }

                var totalCount=0;

                var outobj = GetPageList<Business, dynamic>(pageIndex, pageSize, out totalCount, conModels, it => new {
                    businessId = it.BusinessId,
                    businessNo = it.BusinessNo,
                    businessName = it.BusinessName,
                    appType = it.AppType,
                    status = it.Status,
                    createdAt = SqlFunc.ToString(it.CreatedAt)
                }, orderbyList).ToList();


                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public ResultObject updateBusiness(Business bV0, string[] updateColums = null)
        {
            //判断
            var businessId = bV0.BusinessId;
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;



                if (businessId == 0)
                {
                    int id = Add<Business>(bV0);
                    bV0.BusinessId = id;
                    if (id > 0)
                        resultobj= new ResultObject() { Flag = 1, Message = "添加成功!", Result = bV0 };
                    else
                        resultobj= new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

                }
                else
                {
                    bV0.UpdateTime = DateTime.Now;

                    Array.Resize(ref updateColums, updateColums.Length + 1);
                    updateColums[updateColums.Length - 1] = "updateTime";

                    bool isSuccess = Update<Business>(bV0, updateColums);
                    if (isSuccess)
                        resultobj= new ResultObject() { Flag = 1, Message = "更新成功!", Result = bV0 };
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

        public ResultObject getBusinessById(int id)
        {
            try
            {
                var outobj = GetById<Business>(id);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }



        public ResultObject deleteBusiness(int id)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;
                bool isSuccess = Delete<Business>(id);
                if (isSuccess)
                {
                    resultobj= new ResultObject() { Flag = 1, Message = "删除成功!", Result = id };
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
