using publicClassLibrary.Helpers;
using SqlSugar;

namespace publicClassLibrary.Entitys
{
    [SugarTable("mall_admin_accounts","后台用户表")]
    public class Adminaccounts
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int AdminId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }


        [SugarColumn(ColumnName = "UserNo", ColumnDescription = "用户编号", Length = 50)]
        public string UserNo { get; set; }


        [SugarColumn(ColumnName = "UserName", ColumnDescription = "用户名", Length = 50)]
        public string UserName { get; set; }



        [SugarColumn(ColumnName = "Password", ColumnDescription = "密码(加密)", Length = 255)]
        public string Password { get; set; } = MD5Helper.GetMD5("654321"); //初始密码为654321

        [SugarColumn(ColumnName = "RealName", ColumnDescription = "真实姓名", Length = 50)]
        public string RealName { get; set; }

        [SugarColumn(ColumnName = "Phone", ColumnDescription = "联系电话", Length = 20)]
        public string Phone { get; set; }

        [SugarColumn(ColumnName = "Email", ColumnDescription = "邮箱", Length = 100)]
        public string Email { get; set; }

        [SugarColumn(ColumnName = "Avatar", ColumnDescription = "头像", Length = 512)]
        public string Avatar { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:1-正常 0-禁用")]
        public int Status { get; set; }

        [SugarColumn(ColumnName = "IsSuperAdmin", ColumnDescription = "是否超管:1-是 0-否")]
        public int IsSuperAdmin { get; set; }

        [SugarColumn(ColumnName = "LastLoginTime", ColumnDescription = "最后登录时间")]
        public DateTime LastLoginTime { get; set; }

        [SugarColumn(ColumnName = "LastLoginIp", ColumnDescription = "最后登录IP", Length = 45)]
        public string LastLoginIp { get; set; }

        [SugarColumn(ColumnName = "LoginCount", ColumnDescription = "登录次数")]
        public int LoginCount { get; set; }

        [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 255)]
        public string Remark { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }

    }
}
