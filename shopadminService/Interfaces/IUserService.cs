using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using shopadminService.Services;

namespace shopadminService.Interfaces
{
    public interface IUserService
    {
        Adminaccounts? postLogin(string userName,string password, int appType);

        void updateLoginInfo(Adminaccounts entity);

        bool changeUserPassword(string userName,string oldPassword, string newPassword, int appType);


        List<dynamic> getAdminaccountsPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? status, out int totalCount);

        

        ResultObject updateUsers(Adminaccounts cV0, string[] updateColums = null);

        ResultObject deleteUsers(int id);
    }
}
