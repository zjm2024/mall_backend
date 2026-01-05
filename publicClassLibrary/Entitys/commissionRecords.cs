using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 15. 分佣记录表 (mall_commission_records)
    // ----------------------------
    [SugarTable("mall_commission_records")]
    public class CommissionRecords
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int RecordId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单ID")]
        public int OrderId { get; set; }

        [SugarColumn(ColumnName = "PersonalID", ColumnDescription = "分销员ID")]
        public int PersonalID { get; set; }

        [SugarColumn(ColumnName = "Level", ColumnDescription = "分销层级:1-一级 2-二级")]
        public int Level { get; set; }

        [SugarColumn(ColumnName = "Amount", ColumnDescription = "分佣金额")]
        public decimal Amount { get; set; }

        [SugarColumn(ColumnName = "CommissionRate", ColumnDescription = "分佣比例")]
        public decimal CommissionRate { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:1-待结算 2-已结算 3-已提现")]
        public int Status { get; set; }

        [SugarColumn(ColumnName = "SettleTime", ColumnDescription = "结算时间")]
        public DateTime SettleTime { get; set; }

        [SugarColumn(ColumnName = "WithdrawTime", ColumnDescription = "提现时间")]
        public DateTime WithdrawTime { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }
    }
}
