using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
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
            var result = GetList<Adminaccounts>(it => it.AppType == appType && it.Username == userName && it.Password == password);
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
            var result = GetList<Adminaccounts>(it => it.AppType == appType && it.Username == userName && it.Password == oldPassword);
            if (result.Count > 0)
            {
                Adminaccounts entity = result[0];
                entity.Password=newPassword; 
               return Update<Adminaccounts>(entity, it => new { it.Password });
            }
            else
                return false;


        }

        public List<Adminaccounts> getAdminaccountsPageList(int pageIndex, int pageSize, int appType, out int totalCount)
        {
            //加排序
            List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "CreateTime", OrderByType = OrderByType.Desc });
            var list = GetPageList<Adminaccounts>(pageIndex, pageSize, out totalCount, it => it.AppType == appType, orderbyList);
            return list;
        }
    }
}

