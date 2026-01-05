using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 13. 账户表 (mall_accounts)
    // ----------------------------
    [SugarTable("mall_accounts")]
    public class Accounts
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int AccountId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "OwnerType", ColumnDescription = "所有者类型:1-用户 2-商家")]
        public int OwnerType { get; set; }

        [SugarColumn(ColumnName = "OwnerId", ColumnDescription = "所有者ID")]
        public int OwnerId { get; set; }

        [SugarColumn(ColumnName = "Balance", ColumnDescription = "可用余额")]
        public decimal Balance { get; set; }

        [SugarColumn(ColumnName = "FrozenAmount", ColumnDescription = "冻结金额")]
        public decimal FrozenAmount { get; set; }

        [SugarColumn(ColumnName = "TotalIncome", ColumnDescription = "总收入")]
        public decimal TotalIncome { get; set; }

        [SugarColumn(ColumnName = "TotalExpense", ColumnDescription = "总支出")]
        public decimal TotalExpense { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:1-正常 0-冻结")]
        public int Status { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }
    }

}
