using Dm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace publicClassLibrary.Entitys
{
    [SugarTable("mall_products")]
    public class products
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int ProductId { get; set; }

        [SugarColumn(ColumnName = "AppType",ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }

        [SugarColumn(ColumnName = "CategoryId", ColumnDescription = "分类ID")]
        public int CategoryId { get; set; }

        [SugarColumn(ColumnName = "ProductName", ColumnDescription = "商品名称", Length = 255)]
        public string ProductName { get; set; }

        [SugarColumn(ColumnName = "ProductImage", ColumnDescription = "商品主图URL", Length = 512)]
        public string ProductImage { get; set; }

        [SugarColumn(ColumnName = "ProductImages", ColumnDescription = "商品图片集(JSON格式)", Length = int.MaxValue)]
        public string ProductImages { get; set; }

        [SugarColumn(ColumnName = "ProductContent", ColumnDescription = "商品详情(HTML)", Length = int.MaxValue)]
        public string ProductContent { get; set; }


        [SugarColumn(ColumnName = "OriginalPrice", ColumnDescription = "原价")]
        public decimal OriginalPrice { get; set; }

        [SugarColumn(ColumnName = "CurrentPrice", ColumnDescription = "当前售价")]
        public decimal CurrentPrice { get; set; }


        [SugarColumn(ColumnName = "TotalStock", ColumnDescription = "总库存(0表示不限制)")]
        public int TotalStock { get; set; }

        [SugarColumn(ColumnName = "PerPersonLimit", ColumnDescription = "每人限购数量(0表示不限制)")]
        public int PerPersonLimit { get; set; }

        [SugarColumn(ColumnName = "Sales", ColumnDescription = "总销量")]
        public int Sales { get; set; }

        [SugarColumn(ColumnName = "ShowPrice", ColumnDescription = "是否显示价格:1-显示 0-隐藏")]
        public int ShowPrice { get; set; }

        [SugarColumn(ColumnName = "ProductStatus", ColumnDescription = "产品状态:1-上架 0-下架")]
        public int ProductStatus { get; set; }

        [SugarColumn(ColumnName = "SortOrder", ColumnDescription = "排序")]
        public int SortOrder { get; set; }

        [SugarColumn(ColumnName = "CommissionEnabled", ColumnDescription = "是否开启返佣:1-开启 0-关闭")]
        public int CommissionEnabled { get; set; }

        [SugarColumn(ColumnName = "FirstLevelRate", ColumnDescription = "一级分佣比例(%)")]
        public decimal FirstLevelRate { get; set; }

        [SugarColumn(ColumnName = "SecondLevelRate", ColumnDescription = "二级分佣比例(%)")]
        public decimal SecondLevelRate { get; set; }

        [SugarColumn(ColumnName = "AntiRefresh", ColumnDescription = "防刷单:1-开启 0-关闭")]
        public int AntiRefresh { get; set; }

        [SugarColumn(ColumnName = "MaxDailyOrders", ColumnDescription = "每日最大订单数(0表示不限)")]
        public int MaxDailyOrders { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }


    }
}
