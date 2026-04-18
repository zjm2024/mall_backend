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
    public  class PersonalService : BaseService, IPersonalService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public PersonalService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
        }

        public ResultObject getPersonalPageList(int pageIndex, int pageSize, int appType,int businessId, string? searchKey, int? status)
        {
            try
            {
                //加排序
                List<OrderByModel> orderbyList = OrderByModel.Create(
                   new OrderByModel() { FieldName = "CreatedAt", OrderByType = OrderByType.Desc });

                //加查询条件
                var conModels = new List<IConditionalModel>();
                conModels.Add(new ConditionalModel { FieldName = "AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });
                conModels.add(new ConditionalModel { FieldName = "BusinessId", ConditionalType = ConditionalType.Equal, FieldValue = businessId.toString() });

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
                       new ConditionalModel(){FieldName ="Phone",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName ="Name",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),


                    }
                    });
                }

                var totalCount=0;

                var outobj = GetPageList<Personal, dynamic>(pageIndex, pageSize, out totalCount, conModels, it => new {
                    personalId = it.PersonalId,
                    name = it.Name,
                    phone = it.Phone,
                    email=it.Email,
                    weChat=it.WeChat,
                    headimg=it.Headimg,
                    business = it.Business,
                    position = it.Position,
                    appType = it.AppType,
      
                    createdAt = SqlFunc.ToString(it.CreatedAt)
                }, orderbyList).ToList();


                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public ResultObject updatePersonal(Personal pV0, string[] updateColums = null)
        {
            //判断
            var personalId = pV0.PersonalId;
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;



                if (personalId == 0)
                {
                    int id = Add<Personal>(pV0);
                    pV0.PersonalId = id;
                    if (id > 0)
                        resultobj= new ResultObject() { Flag = 1, Message = "添加成功!", Result = pV0 };
                    else
                        resultobj= new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

                }
                else
                {
                    pV0.UpdateTime = DateTime.Now;

                    Array.Resize(ref updateColums, updateColums.Length + 1);
                    updateColums[updateColums.Length - 1] = "updateTime";

                    bool isSuccess = Update<Personal>(pV0, updateColums);
                    if (isSuccess)
                        resultobj= new ResultObject() { Flag = 1, Message = "更新成功!", Result = pV0 };
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

        public ResultObject getPersonalById(int id)
        {
            try
            {
                var outobj = GetById<Personal>(id);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }



        public ResultObject deletePersonal(int id)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;
                bool isSuccess = Delete<Personal>(id);
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
