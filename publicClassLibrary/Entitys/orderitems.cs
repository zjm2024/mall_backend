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

        [SugarColumn(ColumnName = "OrderNo", ColumnDescription = "订单号", Length = 50)]
        public string OrderNo { get; set; }

        [SugarColumn(ColumnName = "SubOrderId", ColumnDescription = "子订单ID")]
        public int SubOrderId { get; set; }

        [SugarColumn(ColumnName = "SubOrderNo", ColumnDescription = "子订单号", Length = 50)]
        public string SubOrderNo { get; set; }


        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }


        [SugarColumn(ColumnName = "PersonalId", ColumnDescription = "买家ID")]
        public int PersonalId { get; set; }

        [SugarColumn(ColumnName = "ProductId", ColumnDescription = "商品ID")]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }

        [SugarColumn(ColumnName = "SpecId", ColumnDescription = "规格ID")]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "ProductName", ColumnDescription = "商品名称", Length = 255)]
        public string ProductName { get; set; }

        [SugarColumn(ColumnName = "Spec1Name", ColumnDescription = "规格名称1", Length = 100)]
        public string Spec1Name { get; set; }

        [SugarColumn(ColumnName = "Spec1Value", ColumnDescription = "规格值1", Length = 100)]
        public string Spec1Value { get; set; }

        [SugarColumn(ColumnName = "Spec2Name", ColumnDescription = "规格名称2", Length = 100)]
        public string Spec2Name { get; set; }

        [SugarColumn(ColumnName = "Spec2Value", ColumnDescription = "规格值2", Length = 100)]
        public string Spec2Value { get; set; }

        [SugarColumn(ColumnName = "Spec3Name", ColumnDescription = "规格名称3", Length = 100)]
        public string Spec3Name { get; set; }

        [SugarColumn(ColumnName = "Spec3Value", ColumnDescription = "规格值3", Length = 100)]
        public string Spec3Value { get; set; }



        [SugarColumn(ColumnName = "Quantity", ColumnDescription = "购买数量")]
        public int Quantity { get; set; }

        [SugarColumn(ColumnName = "OriginalPrice", ColumnDescription = "原价")]
        public decimal OriginalPrice { get; set; }

        [SugarColumn(ColumnName = "UnitPrice", ColumnDescription = "单价")]
        public decimal UnitPrice { get; set; }

        [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "小计")]
        public decimal TotalAmount { get; set; }

        [SugarColumn(ColumnName = "PayAmount", ColumnDescription = "应付")]
        public decimal PayAmount { get; set; }

        

        [SugarColumn(ColumnName = "ActivityType", ColumnDescription = "活动类型")]
        public int ActivityType { get; set; }

        [SugarColumn(ColumnName = "ActivityId", ColumnDescription = "活动ID")]
        public int ActivityId { get; set; }

        [SugarColumn(ColumnName = "RefundStatus", ColumnDescription = "退款状态:0-无退款 1-退款中 2-已退款 3-部分退款")]
        public int RefundStatus { get; set; }

        [SugarColumn(ColumnName = "RefundAmount", ColumnDescription = "退款金额")]
        public decimal RefundAmount { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }
}
