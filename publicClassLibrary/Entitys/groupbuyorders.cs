using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 10. 团购订单表 (mall_group_buy_orders)
    // ----------------------------
    [SugarTable("mall_group_buy_orders")]
    public class GroupBuyOrders
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int GroupOrderId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "GroupBuyId", ColumnDescription = "团购活动ID")]
        public int GroupBuyId { get; set; }

        [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单ID")]
        public int OrderId { get; set; }

        [SugarColumn(ColumnName = "PersonalID", ColumnDescription = "用户ID")]
        public int PersonalID { get; set; }

        [SugarColumn(ColumnName = "SpecId", ColumnDescription = "规格ID")]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "Quantity", ColumnDescription = "购买数量")]
        public int Quantity { get; set; }

        [SugarColumn(ColumnName = "IsCreator", ColumnDescription = "是否团长:1-是 0-否")]
        public int IsCreator { get; set; }

        [SugarColumn(ColumnName = "JoinTime", ColumnDescription = "参团时间")]
        public DateTime JoinTime { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:0-待成团 1-已成团 2-已退款")]
        public int Status { get; set; }

        [SugarColumn(ColumnName = "RefundAmount", ColumnDescription = "退款金额")]
        public decimal RefundAmount { get; set; }

        [SugarColumn(ColumnName = "RefundTime", ColumnDescription = "退款时间")]
        public DateTime RefundTime { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
