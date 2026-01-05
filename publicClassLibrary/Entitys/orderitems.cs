using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 9. 订单明细表 (mall_order_items)
    // ----------------------------
    [SugarTable("mall_order_items")]
    public class OrderItems
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int OrderItemId { get; set; }

        [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单ID")]
        public int OrderId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "ProductId", ColumnDescription = "商品ID")]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "SpecId", ColumnDescription = "规格ID")]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "ProductName", ColumnDescription = "商品名称(快照)", Length = 255)]
        public string ProductName { get; set; }

        [SugarColumn(ColumnName = "SpecName", ColumnDescription = "规格名称(快照)", Length = 100)]
        public string SpecName { get; set; }

        [SugarColumn(ColumnName = "SpecValue", ColumnDescription = "规格值(快照)", Length = 100)]
        public string SpecValue { get; set; }

        [SugarColumn(ColumnName = "Quantity", ColumnDescription = "购买数量")]
        public int Quantity { get; set; }

        [SugarColumn(ColumnName = "OriginalPrice", ColumnDescription = "原价")]
        public decimal OriginalPrice { get; set; }

        [SugarColumn(ColumnName = "UnitPrice", ColumnDescription = "单价")]
        public decimal UnitPrice { get; set; }

        [SugarColumn(ColumnName = "TotalPrice", ColumnDescription = "小计")]
        public decimal TotalPrice { get; set; }

        [SugarColumn(ColumnName = "ActivityType", ColumnDescription = "活动类型")]
        public int ActivityType { get; set; }

        [SugarColumn(ColumnName = "ActivityId", ColumnDescription = "活动ID")]
        public int ActivityId { get; set; }

        [SugarColumn(ColumnName = "RefundStatus", ColumnDescription = "退款状态:0-无退款 1-退款中 2-已退款 3-部分退款")]
        public int RefundStatus { get; set; }

        [SugarColumn(ColumnName = "RefundAmount", ColumnDescription = "退款金额")]
        public decimal RefundAmount { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }
    }
}
