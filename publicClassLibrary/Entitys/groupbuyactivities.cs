using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 7. 团购活动表 (mall_group_buy_activities)
    // ----------------------------
    [SugarTable("mall_group_buy_activities")]
    public class GroupBuyActivities
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int GroupBuyId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "ProductId", ColumnDescription = "商品ID")]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "GroupBuyPrice", ColumnDescription = "团购价格")]
        public decimal GroupBuyPrice { get; set; }

        [SugarColumn(ColumnName = "DiscountRate", ColumnDescription = "折扣比例")]
        public int DiscountRate { get; set; }

        [SugarColumn(ColumnName = "PeopleNumber", ColumnDescription = "成团人数(2-15人)")]
        public int PeopleNumber { get; set; }

        [SugarColumn(ColumnName = "MinPeople", ColumnDescription = "最小成团人数")]
        public int MinPeople { get; set; }

        [SugarColumn(ColumnName = "MaxPeople", ColumnDescription = "最大参团人数")]
        public int MaxPeople { get; set; }

        [SugarColumn(ColumnName = "ExpireHours", ColumnDescription = "成团期限(小时)")]
        public int ExpireHours { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:1-进行中 2-已结束 3-已关闭")]
        public int Status { get; set; }

        [SugarColumn(ColumnName = "RefundRate", ColumnDescription = "失败退款比例")]
        public decimal RefundRate { get; set; }

        [SugarColumn(ColumnName = "AutoComplete", ColumnDescription = "自动成团:1-是 0-否")]
        public int AutoComplete { get; set; }

        [SugarColumn(ColumnName = "PerPersonLimit", ColumnDescription = "每人限购")]
        public int PerPersonLimit { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }
    }
}
