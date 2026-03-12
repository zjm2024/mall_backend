using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 14. 账户流水表 (mall_account_transactions)
    // ----------------------------
    [SugarTable("mall_account_transactions")]
    public class AccountTransactions
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int TransactionId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "AccountId", ColumnDescription = "账户ID")]
        public int AccountId { get; set; }

        [SugarColumn(ColumnName = "Amount", ColumnDescription = "金额(正数收入，负数支出)")]
        public decimal Amount { get; set; }

        [SugarColumn(ColumnName = "Type", ColumnDescription = "类型:1-订单收入 2-提现 3-充值 4-退款 5-分佣 6-退款支出")]
        public int Type { get; set; }

        [SugarColumn(ColumnName = "RelatedId", ColumnDescription = "关联ID")]
        public int RelatedId { get; set; }

        [SugarColumn(ColumnName = "BalanceBefore", ColumnDescription = "操作前余额")]
        public decimal BalanceBefore { get; set; }

        [SugarColumn(ColumnName = "BalanceAfter", ColumnDescription = "操作后余额")]
        public decimal BalanceAfter { get; set; }

        [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 255)]
        public string Remark { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
