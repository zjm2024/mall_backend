using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    //  商家表 (mall_carts)
    // ----------------------------
    [SugarTable("mall_business")]
    public class Business
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int BusinessId { get; set; }

        [SugarColumn(ColumnName = "BusinessName", ColumnDescription = "商家名称" ,Length = 255)]
        public string BusinessName { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }


        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }
}
