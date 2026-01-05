using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 2. 购物车表 (mall_carts)
    // ----------------------------
    [SugarTable("mall_carts")]
    public class Carts
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int CartId { get; set; }

        [SugarColumn(ColumnName = "PersonalID", ColumnDescription = "用户ID")]
        public int PersonalID { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "ProductId", ColumnDescription = "商品ID")]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "SpecId", ColumnDescription = "规格ID")]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "Quantity", ColumnDescription = "购买数量")]
        public int Quantity { get; set; }

        [SugarColumn(ColumnName = "Selected", ColumnDescription = "是否选中:1-是 0-否")]
        public int Selected { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }
    }
}
