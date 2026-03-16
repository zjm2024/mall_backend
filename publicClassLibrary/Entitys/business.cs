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

        

        [SugarColumn(ColumnName = "BusinessNo", ColumnDescription = "商家编号", Length = 50)]
        public string BusinessNo { get; set; }

        [SugarColumn(ColumnName = "BusinessName", ColumnDescription = "商家名称" ,Length = 255)]
        public string BusinessName { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:0-审批，1-上线，2-下线，3-删除")]
        public int Status { get; set; }


        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }
}
