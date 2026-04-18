using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    //  数据字典表 (mall_datadicts)
    // ----------------------------
    [SugarTable("mall_datadicts")]
    public class DataDicts
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int DataDictId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }

        [SugarColumn(ColumnName = "Code", ColumnDescription = "字典编号", Length = 100)]
        public string Code { get; set; }

        [SugarColumn(ColumnName = "Name", ColumnDescription = "字典名称" ,Length = 100)]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "Value", ColumnDescription = "数据值", Length = 5000)]
        public string Value { get; set; }




        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:0-审批，1-上线，2-下线，3-删除")]
        public int Status { get; set; }


        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }


    public class SeckillTimers
    {
   
        public string SeckillTime { get; set; }


        public int SeckillMinutes { get; set; }



        public int SortOrder { get; set; }

        public int index { get; set; }


    }
}
