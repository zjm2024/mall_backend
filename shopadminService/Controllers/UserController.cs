using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopadminService.Interfaces;
using shopadminService.Services;
using System.Text.Json;
using System.Web;

namespace shopadminService.Controllers
{
    [Anonymous]
    [ApiController]
    [Route("shopadminApi/User/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userservice;
        public UserController(IHttpContextAccessor httpContext,ILogger<UserController> logger, IUserService userservice)
        {
            _httpContext = httpContext;
            _logger = logger;
            _userservice = userservice;
        }

        [HttpPost]
        public ResultObject validAccount([FromBody] JsonElement formData)
        {

            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);
   
            string userName = jsonElement.GetProperty("UserName").ToString();
            string password = jsonElement.GetProperty("Password").ToString();
            int appType = Convert.ToInt32(jsonElement.GetProperty("AppType").ToString());

            //加密密码
            password = MD5Helper.GetMD5(password);

            var result =_userservice.postLogin(userName, password, appType);

            if (result!=null)
            {
                var clientIp = _httpContext.HttpContext.Connection.RemoteIpAddress;
                if (clientIp != null)
                {
                    result.LastLoginIp = clientIp.ToString();
                }

                result.LastLoginTime = DateTime.Now;
                result.LoginCount = result.LoginCount + 1;
                result.UpdateTime = DateTime.Now;
                _userservice.updateLoginInfo(result);
                return new ResultObject() { Flag = 1, Message = "验证成功!", Result = new
                { appType=result.AppType, businessId=result.BusinessId, avatar=result.Avatar ,phone=result.Phone,email=result.Email,
                    userNo =result.UserNo, userName =result.UserName,realName=result.RealName , isSuperAdmin=result.IsSuperAdmin} };

            }
            else
            {
                return new ResultObject() { Flag = 0, Message = "验证失败!", Result = null };
            }
        }





        /// <summary>
        /// 查询类型传递 params ，修改 新增 传递 data
        /// 根据pageIndex,pageSize 分页获取实体，自动带输出参数返回总记录
        /// </summary>
        [HttpPost]
        public ResultObject getAdminaccountsPageList([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("params", out jValue)) ? "" : jValue.GetRawText());
            JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);

            int pageIndex = Convert.ToInt32(jsonElement.GetProperty("pageIndex").ToString());
            int pageSize = Convert.ToInt32(jsonElement.GetProperty("pageSize").ToString());
            int appType = Convert.ToInt32(jsonElement.GetProperty("appType").ToString());
            JsonElement outjValue;
            int? status = ((!jsonElement.TryGetProperty("status", out outjValue)) ? null : outjValue.GetInt32());
            string? searchKey = ((!jsonElement.TryGetProperty("searchKey", out outjValue)) ? null : outjValue.GetString());

            int totalCount = 0;
            var outobj = _userservice.getAdminaccountsPageList(pageIndex, pageSize, appType, searchKey, status, out totalCount);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount, Subsidiary = 1 };
        }

        /// <summary>
        /// 改用户密码
        /// </summary>
        /// <param name="userNo">用户账号</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        [HttpPost]
        public ResultObject changeUserPassword([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);

            string userNo = jsonElement.GetProperty("userNo").ToString();
            string oldPassword = jsonElement.GetProperty("oldPassword").ToString();
            string newPassword = jsonElement.GetProperty("newPassword").ToString();
            int appType = Convert.ToInt32(jsonElement.GetProperty("appType").ToString());

            oldPassword = MD5Helper.GetMD5(oldPassword);
            newPassword = MD5Helper.GetMD5(newPassword);

  
            bool result = _userservice.changeUserPassword(userNo, oldPassword, newPassword, appType);
            if (result)
                return new ResultObject() { Flag = 1, Message = "修改成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "密码错误，请重新输入!", Result = null };
        }


        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userNo">用户账号</param>
        /// <returns></returns>
        [HttpPost]
        public ResultObject resetUserPassword([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            JsonElement jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);

            string userNo = jsonElement.GetProperty("userNo").ToString();
          
            int appType = Convert.ToInt32(jsonElement.GetProperty("appType").ToString());


            var iniPassword = MD5Helper.GetMD5(MD5Helper.IniPassword);

            bool result = _userservice.resetUserPassword(userNo, iniPassword, appType);
            if (result)
                return new ResultObject() { Flag = 1, Message = "重置成功!", Result = null };
            else
                return new ResultObject() { Flag = 0, Message = "重置失败!", Result = null };
        }


        /// <summary>
        /// 更新或插入用户
        /// </summary>
        [HttpPost]
        public ResultObject updateUsers([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(Adminaccounts));
            if (entity == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }

            //获取json中的修改字段
            List<string> listColums = new List<string>();

            JObject jsonobj = JObject.Parse(json);
            foreach (JProperty prop in jsonobj.Properties())
            {
                listColums.Add(prop.Name);

            }
            string[] updateColums = listColums.ToArray();



            return _userservice.updateUsers((Adminaccounts)entity, updateColums);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpGet]
        public ResultObject deleteUsers(int id)
        {
            return _userservice.deleteUsers(id);
        }



    }
}
