using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 17. 库存变更日志 (mall_stock_logs)
    // ----------------------------
    [SugarTable("mall_stock_logs")]
    public class StockLogs
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int LogId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "ProductId", ColumnDescription = "商品ID")]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "SpecId", ColumnDescription = "规格ID")]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "ChangeQuantity", ColumnDescription = "变更数量(正数增加，负数减少)")]
        public int ChangeQuantity { get; set; }

        [SugarColumn(ColumnName = "ChangeType", ColumnDescription = "变更类型:1-下单扣减 2-取消返还 3-管理员调整 4-退货返还 5-退款返还")]
        public int ChangeType { get; set; }

        [SugarColumn(ColumnName = "OrderId", ColumnDescription = "关联订单")]
        public int OrderId { get; set; }

        [SugarColumn(ColumnName = "Operator", ColumnDescription = "操作人", Length = 50)]
        public string Operator { get; set; }

        [SugarColumn(ColumnName = "BalanceBefore", ColumnDescription = "操作前库存")]
        public int BalanceBefore { get; set; }

        [SugarColumn(ColumnName = "BalanceAfter", ColumnDescription = "操作后库存")]
        public int BalanceAfter { get; set; }

        [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 255)]
        public string Remark { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
