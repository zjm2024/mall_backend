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
        public UserService(SqlSugarHelper dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public Adminaccounts? postLogin(string userName, string password, int appType)
        {
            var result = GetList<Adminaccounts>(it => it.AppType == appType && it.UserName == userName && it.Password == password);
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

        public bool changeUserPassword(string userName, string oldPassword, string newPassword ,int appType)
        {
            //修改用户密码
            var result = GetList<Adminaccounts>(it => it.AppType == appType && it.UserName == userName && it.Password == oldPassword);
            if (result.Count > 0)
            {
                Adminaccounts entity = result[0];
                entity.Password=newPassword; 
               return Update<Adminaccounts>(entity, it => new { it.Password });
            }
            else
                return false;


        }

        public List<dynamic> getAdminaccountsPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status, out int totalCount)
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


            var list = GetPageList<Adminaccounts, dynamic>(pageIndex, pageSize, out totalCount, conModels, it => new {
                adminId =it.AdminId,
                isSuperAdmin =it.IsSuperAdmin,
                userNo= it.UserNo,
                userName =it.UserName,
                realName =it.RealName,
                avatar =it.Avatar,
                appType =it.AppType,
                status=it.Status,
                createTime =SqlFunc.ToString(it.CreateTime) 
            }, orderbyList);
            return list;
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

            if (adminId == 0)
            {
                int id = Add<Adminaccounts>(cVO);
                cVO.AdminId = id;
                if (id > 0)
                    return new ResultObject() { Flag = 1, Message = "添加成功!", Result = cVO };
                else
                    return new ResultObject() { Flag = 0, Message = "添加失败!", Result = null };

            }
            else
            {

                bool isSuccess = Update<Adminaccounts>(cVO, updateColums);
                if (isSuccess)
                    return new ResultObject() { Flag = 1, Message = "更新成功!", Result = cVO };
                else
                    return new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
            }
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户id</param>
        public ResultObject deleteUsers(int id)
        {
            try
            {

                bool isSuccess = Delete<Adminaccounts>(id);
                if (isSuccess)
                {
                    return new ResultObject() { Flag = 1, Message = "删除成功!", Result = id };
                }
                else
                {
                    return new ResultObject() { Flag = 1, Message = "删除失败!", Result = null };
                }



            }
            catch(Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "删除失败!"+ex.toString(), Result = null };
            }
        }
    








}
}

