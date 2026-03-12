using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 8. 订单主表 (mall_orders)
    // ----------------------------
    [SugarTable("mall_orders")]
    public class Orders
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int OrderId { get; set; }

        [SugarColumn(ColumnName = "OrderNo", ColumnDescription = "订单号", Length = 50)]
        public string OrderNo { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "PersonalID", ColumnDescription = "买家ID")]
        public int PersonalId { get; set; }


        [SugarColumn(ColumnName = "TotalAmount", ColumnDescription = "订单总金额")]
        public decimal TotalAmount { get; set; }

        [SugarColumn(ColumnName = "DiscountAmount", ColumnDescription = "优惠金额")]
        public decimal DiscountAmount { get; set; }

        [SugarColumn(ColumnName = "PayAmount", ColumnDescription = "实际支付金额")]
        public decimal PayAmount { get; set; }

        [SugarColumn(ColumnName = "PaymentMethod", ColumnDescription = "支付方式:0-微信 1-余额 2-混合")]
        public int PaymentMethod { get; set; }

        [SugarColumn(ColumnName = "BalanceAmount", ColumnDescription = "余额支付金额")]
        public decimal BalanceAmount { get; set; }

        [SugarColumn(ColumnName = "WxPayAmount", ColumnDescription = "微信支付金额")]
        public decimal WxPayAmount { get; set; }

        [SugarColumn(ColumnName = "TransactionId", ColumnDescription = "微信交易号", Length = 255)]
        public string TransactionId { get; set; }

        [SugarColumn(ColumnName = "PayStatus", ColumnDescription = "支付状态:0-待支付 1-已支付 2-已取消")]
        public int PayStatus { get; set; }

        [SugarColumn(ColumnName = "PayTime", ColumnDescription = "支付时间")]
        public DateTime PayTime { get; set; }


        [SugarColumn(ColumnName = "TotalCount", ColumnDescription = "总商品数量")]
        public int TotalCount { get; set; }

        

        [SugarColumn(ColumnName = "ActivityType", ColumnDescription = "活动类型:0-普通 1-秒杀 2-团购")]
        public int ActivityType { get; set; }

        [SugarColumn(ColumnName = "ActivityId", ColumnDescription = "活动ID")]
        public int ActivityId { get; set; }

        [SugarColumn(ColumnName = "GroupBuyStatus", ColumnDescription = "团购状态:0-普通 1-待成团 2-已成团 3-失败")]
        public int GroupBuyStatus { get; set; }

        [SugarColumn(ColumnName = "FirstLevelPersonalID", ColumnDescription = "一级分销员ID")]
        public int FirstLevelPersonalID { get; set; }

        [SugarColumn(ColumnName = "SecondLevelPersonalID", ColumnDescription = "二级分销员ID")]
        public int SecondLevelPersonalID { get; set; }

        [SugarColumn(ColumnName = "FirstLevelAmount", ColumnDescription = "一级分佣金额")]
        public decimal FirstLevelAmount { get; set; }

        [SugarColumn(ColumnName = "SecondLevelAmount", ColumnDescription = "二级分佣金额")]
        public decimal SecondLevelAmount { get; set; }

        [SugarColumn(ColumnName = "OrderStatus", ColumnDescription = "订单状态:0-待处理 1-已发货 2-已完成 3-已关闭")]
        public int OrderStatus { get; set; }

        [SugarColumn(ColumnName = "ShippingNo", ColumnDescription = "物流单号", Length = 100)]
        public string ShippingNo { get; set; }

        [SugarColumn(ColumnName = "ShippingTime", ColumnDescription = "发货时间")]
        public DateTime ShippingTime { get; set; }

        [SugarColumn(ColumnName = "CompleteTime", ColumnDescription = "完成时间")]
        public DateTime CompleteTime { get; set; }

        [SugarColumn(ColumnName = "ReceiverName", ColumnDescription = "收货人姓名", Length = 50)]
        public string ReceiverName { get; set; }

        [SugarColumn(ColumnName = "ReceiverPhone", ColumnDescription = "收货人电话", Length = 20)]
        public string ReceiverPhone { get; set; }

        [SugarColumn(ColumnName = "ReceiverAddress", ColumnDescription = "收货地址", Length = 255)]
        public string ReceiverAddress { get; set; }

        [SugarColumn(ColumnName = "RiskLevel", ColumnDescription = "风险等级:0-正常 1-可疑 2-高风险")]
        public int RiskLevel { get; set; }

        [SugarColumn(ColumnName = "RiskReason", ColumnDescription = "风险原因", Length = 255)]
        public string RiskReason { get; set; }

        [SugarColumn(ColumnName = "Remark", ColumnDescription = "订单备注")]
        public string Remark { get; set; }

        
        [SugarColumn(ColumnName = "CancelTime", ColumnDescription = "取消时间")]
        public DateTime? CancelTime { get; set; } = null;

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;

        /// <summary>
        /// 子订单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<OrdersSubs> OrdersSubs { get; set; } = new List<OrdersSubs>();
    }

}
