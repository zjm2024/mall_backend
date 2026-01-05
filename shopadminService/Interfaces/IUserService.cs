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


        List<Adminaccounts> getAdminaccountsPageList(int pageIndex, int pageSize, int appType, out int totalCount);
 
    }
}
