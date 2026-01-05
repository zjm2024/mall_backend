using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 1. 用户限制表 (mall_user_limits)
    // ----------------------------
    [SugarTable("mall_user_limits")]
    public class UserLimits
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int LimitId { get; set; }

        [SugarColumn(ColumnName = "PersonalID", ColumnDescription = "用户ID")]
        public int PersonalID { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "LimitType", ColumnDescription = "限制类型:1-秒杀限购 2-每日金额 3-每日单数")]
        public int LimitType { get; set; }

        [SugarColumn(ColumnName = "LimitValue", ColumnDescription = "限制阈值")]
        public decimal LimitValue { get; set; }

        [SugarColumn(ColumnName = "CurrentValue", ColumnDescription = "当前值")]
        public decimal CurrentValue { get; set; }

        [SugarColumn(ColumnName = "Date", ColumnDescription = "日期(用于每日重置)")]
        public DateTime Date { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }
    }
}
