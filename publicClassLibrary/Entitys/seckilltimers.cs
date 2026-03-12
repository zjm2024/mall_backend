using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    //  秒杀时间段表 (mall_seckill_timers)
    // ----------------------------
    [SugarTable("mall_seckill_timers")]
    public class SeckillTimers
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int TimerId { get; set; }


        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "SeckillTime", ColumnDescription = "秒杀时间", Length = 50)]
        public string SeckillTime { get; set; }

        [SugarColumn(ColumnName = "SeckillMinutes", ColumnDescription = "秒杀分钟")]
        public int SeckillMinutes { get; set; }


        [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序")]
        public int SortOrder { get; set; }

   

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }
}
