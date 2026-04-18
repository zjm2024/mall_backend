using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    //  员工表 (t_bc_personal)
    // ----------------------------
    [SugarTable("t_bc_personal")]
    public class Personal
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int PersonalId { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }


        [SugarColumn(ColumnName = "Name", ColumnDescription = "姓名", Length = 50)]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "Email", ColumnDescription = "邮箱", Length = 255)]
        public string Email { get; set; }

        [SugarColumn(ColumnName = "WeChat", ColumnDescription = "微信号", Length = 255)]
        public string WeChat { get; set; }

        [SugarColumn(ColumnName = "Headimg", ColumnDescription = "头像", Length = 255)]
        public string Headimg { get; set; }

        [SugarColumn(ColumnName = "Phone", ColumnDescription = "电话" ,Length = 255)]
        public string Phone { get; set; }

        [SugarColumn(ColumnName = "Position", ColumnDescription = "职位", Length = 255)]
        public string Position { get; set; }

        [SugarColumn(ColumnName = "Business", ColumnDescription = "岗位", Length = 255)]
        public string Business { get; set; }


        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }



        [SugarColumn(ColumnName = "Address", ColumnDescription = "地址", Length = 255)]
        public string Address { get; set; }
        

        [SugarColumn(ColumnName = "CreatedAt", ColumnDescription = "创建时间")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }
}
