using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 5. 秒杀活动表 (mall_seckill_activities)
    // ----------------------------
    [SugarTable("mall_seckill_activities")]
    public class SeckillActivities
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int SeckillId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }

        [SugarColumn(ColumnName = "ProductId", ColumnDescription = "商品ID")]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "SeckillPrice", ColumnDescription = "秒杀价格")]
        public decimal SeckillPrice { get; set; }

        [SugarColumn(ColumnName = "DiscountRate", ColumnDescription = "折扣比例(90表示9折)")]
        public int DiscountRate { get; set; }

        [SugarColumn(ColumnName = "ActivityStock", ColumnDescription = "活动库存(必须>0)")]
        public int ActivityStock { get; set; }

        [SugarColumn(ColumnName = "UsedStock", ColumnDescription = "已售库存")]
        public int UsedStock { get; set; }

        [SugarColumn(ColumnName = "SoldPercent", ColumnDescription = "已售百分比")]
        public int SoldPercent { get; set; }
        

        [SugarColumn(ColumnName = "StartTime", ColumnDescription = "开始时间")]
        public DateTime StartTime { get; set; }

        [SugarColumn(ColumnName = "EndTime", ColumnDescription = "结束时间")]
        public DateTime EndTime { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:1-进行中 2-未开始 3-已结束 4-已关闭")]
        public int Status { get; set; }

        [SugarColumn(ColumnName = "PerPersonLimit", ColumnDescription = "每人限购数量")]
        public int PerPersonLimit { get; set; }

        [SugarColumn(ColumnName = "AutoExtend", ColumnDescription = "售罄自动延期:1-是 0-否")]
        public int AutoExtend { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;

        
        //商家名称
        [SugarColumn(IsIgnore = true)]
        public string BusinessName { get; set; }

        //商品编号
        [SugarColumn(IsIgnore = true)]
        public string ProductNo { get; set; }
        //商品名称
        [SugarColumn(IsIgnore = true)]
        public string ProductName { get; set; }
        //商品主图
        [SugarColumn(IsIgnore = true)]
        public string ProductImage { get; set; }
        

    }

}
