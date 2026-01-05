using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 16. 库存预警表 (mall_stock_alerts)
    // ----------------------------
    [SugarTable("mall_stock_alerts")]
    public class StockAlerts
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int AlertId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "ProductId", ColumnDescription = "商品ID")]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "SpecId", ColumnDescription = "规格ID")]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "CurrentStock", ColumnDescription = "当前库存")]
        public int CurrentStock { get; set; }

        [SugarColumn(ColumnName = "AlertThreshold", ColumnDescription = "预警阈值")]
        public int AlertThreshold { get; set; }

        [SugarColumn(ColumnName = "AlertLevel", ColumnDescription = "预警等级:1-警告 2-紧急")]
        public int AlertLevel { get; set; }

        [SugarColumn(ColumnName = "IsHandled", ColumnDescription = "是否处理:0-未处理 1-已处理")]
        public int IsHandled { get; set; }

        [SugarColumn(ColumnName = "HandledBy", ColumnDescription = "处理人", Length = 50)]
        public string HandledBy { get; set; }

        [SugarColumn(ColumnName = "HandleRemark", ColumnDescription = "处理备注", Length = 255)]
        public string HandleRemark { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "HandleTime", ColumnDescription = "处理时间")]
        public DateTime HandleTime { get; set; }
    }
}
