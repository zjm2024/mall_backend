using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 19. 秒杀抢购日志 (mall_seckill_logs)
    // ----------------------------
    [SugarTable("mall_seckill_logs")]
    public class SeckillLogs
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int LogId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "SeckillId", ColumnDescription = "秒杀ID")]
        public int SeckillId { get; set; }

        [SugarColumn(ColumnName = "PersonalID", ColumnDescription = "用户ID")]
        public int PersonalID { get; set; }

        [SugarColumn(ColumnName = "SpecId", ColumnDescription = "规格ID")]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "Quantity", ColumnDescription = "数量")]
        public int Quantity { get; set; }

        [SugarColumn(ColumnName = "Result", ColumnDescription = "结果:1-成功 0-失败")]
        public int Result { get; set; }

        [SugarColumn(ColumnName = "FailReason", ColumnDescription = "失败原因", Length = 100)]
        public string FailReason { get; set; }

        [SugarColumn(ColumnName = "Ip", ColumnDescription = "IP地址", Length = 45)]
        public string Ip { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }
    }

}
