using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 6. 秒杀规格库存表 (mall_seckill_spec_stocks)
    // ----------------------------
    [SugarTable("mall_seckill_spec_stocks")]
    public class SeckillSpecStocks
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "SeckillId", ColumnDescription = "秒杀ID")]
        public int SeckillId { get; set; }

        [SugarColumn(ColumnName = "SpecId", ColumnDescription = "规格ID")]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "ActivityStock", ColumnDescription = "活动库存")]
        public int ActivityStock { get; set; }

        [SugarColumn(ColumnName = "UsedStock", ColumnDescription = "已售库存")]
        public int UsedStock { get; set; }

        [SugarColumn(ColumnName = "SeckillPrice", ColumnDescription = "规格秒杀价")]
        public decimal SeckillPrice { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }

}
