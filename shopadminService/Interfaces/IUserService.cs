using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using shopadminService.Services;

namespace shopadminService.Interfaces
{
    public interface IUserService
    {
        Adminaccounts? postLogin(string userNo,string password, int appType);

        void updateLoginInfo(Adminaccounts entity);

        bool changeUserPassword(string userNo,string oldPassword, string newPassword, int appType);

        bool resetUserPassword(string userNo,string iniPassword,int appType);

        ResultObject getAdminaccountsPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status);

        

        ResultObject updateUsers(Adminaccounts cV0, string[] updateColums = null);

        ResultObject deleteUsers(int id);
    }
}
