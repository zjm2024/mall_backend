using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 4. 商品规格表 (mall_product_specs)
    // ----------------------------
    [SugarTable("mall_product_specs")]
    public class ProductSpecs
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int SpecId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }

        [SugarColumn(ColumnName = "ProductId", ColumnDescription = "商品ID")]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "Spec1Name", ColumnDescription = "规格名称1", Length = 100)]
        public string Spec1Name { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "Spec1Value", ColumnDescription = "规格值1", Length = 100)]
        public string Spec1Value { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "Spec2Name", ColumnDescription = "规格名称2", Length = 100)]
        public string Spec2Name { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "Spec2Value", ColumnDescription = "规格值2", Length = 100)]
        public string Spec2Value { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "Spec3Name", ColumnDescription = "规格名称3", Length = 100)]
        public string Spec3Name { get; set; }=string.Empty;

        [SugarColumn(ColumnName = "Spec3Value", ColumnDescription = "规格值3", Length = 100)]
        public string Spec3Value { get; set; } = string.Empty;

        [SugarColumn(ColumnName = "Price", ColumnDescription = "规格价格")]
        public decimal Price { get; set; }

        [SugarColumn(ColumnName = "Stock", ColumnDescription = "规格库存(0表示不限制)")]
        public int Stock { get; set; }

        [SugarColumn(ColumnName = "Sales", ColumnDescription = "规格销量")]
        public int Sales { get; set; }

        [SugarColumn(ColumnName = "IsDefault", ColumnDescription = "是否默认:1-是 0-否")]
        public int IsDefault { get; set; }

        [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序")]
        public int SortOrder { get; set; }

        [SugarColumn(ColumnName = "Image", ColumnDescription = "规格图片URL", Length = 512)]
        public string Image { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }
}
