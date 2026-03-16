using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
using System.Security.AccessControl;
using System.Text;

namespace shopadminService.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public UserService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
        }

        public Adminaccounts? postLogin(string userNo, string password, int appType)
        {
            var result = GetList<Adminaccounts>(it => it.AppType == appType && it.UserNo == userNo && it.Password == password);
            if (result.Count > 0)
                return result[0];
            else
                return null;
        }

        public void updateLoginInfo(Adminaccounts entity)
        {
            //修改登陆信息到用户表
            Update<Adminaccounts>(entity, it => new { it.LastLoginTime, it.LastLoginIp, it.LoginCount });

        }

        public bool changeUserPassword(string userNo, string oldPassword, string newPassword, int appType)
        {
            //修改用户密码
            var result = GetList<Adminaccounts>(it => it.AppType == appType && it.UserNo == userNo && it.Password == oldPassword);
            if (result.Count > 0)
            {
                Adminaccounts entity = result[0];
                entity.Password = newPassword;
                return Update<Adminaccounts>(entity, it => new { it.Password });
            }
            else
                return false;


        }


        public bool resetUserPassword(string userNo, string iniPassword, int appType)
        {
            //重置用户密码
            var result = GetList<Adminaccounts>(it => it.AppType == appType && it.UserNo == userNo);
            if (result.Count > 0)
            {
                Adminaccounts entity = result[0];
                entity.Password = iniPassword;
                return Update<Adminaccounts>(entity, it => new { it.Password });
            }
            else
                return false;
        }


        public ResultObject getAdminaccountsPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status)
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
                       new ConditionalModel(){FieldName ="UserName",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName ="RealName",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),


                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName="Phone",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()})
                    }
                    });
                }
                var totalCount = 0;

                var outobj = GetPageList<Adminaccounts, dynamic>(pageIndex, pageSize, out totalCount, conModels, it => new
                {
                    adminId = it.AdminId,
                    isSuperAdmin = it.IsSuperAdmin,
                    businessId = it.BusinessId,
                    businessNo = it.BusinessNo,
                    businessName = it.BusinessName,
                    userNo = it.UserNo,
                    userName = it.UserName,
                    realName = it.RealName,
                    avatar = it.Avatar,
                    phone = it.Phone,
                    email = it.Email,
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






        /// <summary>
        /// 添加或更新用户
        /// </summary>
        /// <param name="Adminaccounts">用户cVO</param>

        public ResultObject updateUsers(Adminaccounts cVO, string[] updateColums = null)
        {
            //判断用户编号是否重复
            string userNo = cVO.UserNo;
            int adminId = cVO.AdminId;
            if (adminId == 0)
            {
                var isExist = RecordExist<Adminaccounts, dynamic>(it => it.UserNo == userNo, it => it.AdminId);

                if (isExist)
                {
                    return new ResultObject() { Flag = 0, Message = "用户编号已存在!", Result = null };
                }
            }

            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;

                if (adminId == 0)
                {
                    int id = Add<Adminaccounts>(cVO);
                    cVO.AdminId = id;
                    if (id > 0)
                        resultobj= new ResultObject() { Flag = 1, Message = "添加成功!", Result = cVO };
                    else
                        resultobj= new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

                }
                else
                {

                    bool isSuccess = Update<Adminaccounts>(cVO, updateColums);
                    if (isSuccess)
                        resultobj= new ResultObject() { Flag = 1, Message = "更新成功!", Result = cVO };
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


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户id</param>
        public ResultObject deleteUsers(int id)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;
                bool isSuccess = Delete<Adminaccounts>(id);
                if (isSuccess)
                {
                    resultobj = new ResultObject() { Flag = 1, Message = "删除成功!", Result = id };
                }
                else
                {
                    resultobj = new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
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

