using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    [SugarTable("mall_categories")]
    public class categories
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int CategoryId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "CategoryName", ColumnDescription = "分类名称", Length = 100)]
        public string CategoryName { get; set; }


        [SugarColumn(ColumnName = "ParentId", ColumnDescription = "父分类ID(0表示顶级)")]
        public int ParentId { get; set; }

        [SugarColumn(ColumnName = "Level", ColumnDescription = "分类层级")]
        public int Level { get; set; }

        [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序(降序)")]
        public int SortOrder { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:1-显示 0-隐藏")]
        public int Status { get; set; }

        [SugarColumn(ColumnName = "Icon", ColumnDescription = "分类图标URL", Length = 255)]
        public string Icon { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

    }
}
