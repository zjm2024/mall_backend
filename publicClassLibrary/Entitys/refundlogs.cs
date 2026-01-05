using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 12. 退款日志表 (mall_refund_logs)
    // ----------------------------
    [SugarTable("mall_refund_logs")]
    public class RefundLogs
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int LogId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "RefundId", ColumnDescription = "退款ID")]
        public int RefundId { get; set; }

        [SugarColumn(ColumnName = "Operation", ColumnDescription = "操作:APPLY,AGREE,REJECT,REFUND,CLOSE", Length = 50)]
        public string Operation { get; set; }

        [SugarColumn(ColumnName = "Operator", ColumnDescription = "操作人", Length = 50)]
        public string Operator { get; set; }

        [SugarColumn(ColumnName = "BeforeStatus", ColumnDescription = "操作前状态")]
        public int BeforeStatus { get; set; }

        [SugarColumn(ColumnName = "AfterStatus", ColumnDescription = "操作后状态")]
        public int AfterStatus { get; set; }

        [SugarColumn(ColumnName = "Details", ColumnDescription = "操作详情(JSON)")]
        public string Details { get; set; }

        [SugarColumn(ColumnName = "Remark", ColumnDescription = "备注", Length = 255)]
        public string Remark { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }
    }

}
