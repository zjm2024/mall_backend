using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 18. 订单操作日志 (mall_order_logs)
    // ----------------------------
    [SugarTable("mall_order_logs")]
    public class OrderLogs
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int LogId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单ID")]
        public int OrderId { get; set; }

        [SugarColumn(ColumnName = "Operation", ColumnDescription = "操作:CREATE,PAY,SHIP,CANCEL,REFUND", Length = 50)]
        public string Operation { get; set; }

        [SugarColumn(ColumnName = "Operator", ColumnDescription = "操作人", Length = 50)]
        public string Operator { get; set; }

        [SugarColumn(ColumnName = "BeforeStatus", ColumnDescription = "操作前状态")]
        public int BeforeStatus { get; set; }

        [SugarColumn(ColumnName = "AfterStatus", ColumnDescription = "操作后状态")]
        public int AfterStatus { get; set; }

        [SugarColumn(ColumnName = "Details", ColumnDescription = "操作详情(JSON)")]
        public string Details { get; set; }

        [SugarColumn(ColumnName = "Ip", ColumnDescription = "操作IP", Length = 45)]
        public string Ip { get; set; }

        [SugarColumn(ColumnName = "UserAgent", ColumnDescription = "用户代理")]
        public string UserAgent { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
