using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 订单子表 (mall_orders_subs)
    // ----------------------------
    [SugarTable("mall_orders_subs")]
    public class OrdersSubs
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int SubOrderId { get; set; }

        [SugarColumn(ColumnName = "SubOrderNo", ColumnDescription = "子订单号", Length = 50)]
        public string SubOrderNo { get; set; }

        [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单Id")]
        public int OrderId { get; set; }


        [SugarColumn(ColumnName = "OrderNo", ColumnDescription = "订单号", Length = 50)]
        public string OrderNo { get; set; }


        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "PersonalId", ColumnDescription = "买家ID")]
        public int PersonalId { get; set; }


        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }

        
        [SugarColumn(ColumnName = "SubTotalCount", ColumnDescription = "子订单总商品数")]
        public int SubTotalCount { get; set; }

        [SugarColumn(ColumnName = "SubTotalAmount", ColumnDescription = "子订单总金额")]
        public decimal SubTotalAmount { get; set; }

        [SugarColumn(ColumnName = "SubDiscountAmount", ColumnDescription = "子订单优惠金额")]
        public decimal SubDiscountAmount { get; set; }

        [SugarColumn(ColumnName = "SubPayAmount", ColumnDescription = "子订单实际支付金额")]
        public decimal SubPayAmount { get; set; }

        [SugarColumn(ColumnName = "SubShippingFee", ColumnDescription = "子订单运费")]
        public decimal SubShippingFee { get; set; }

        

        /*
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
        */

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;



        //商家名称
        [SugarColumn(IsIgnore = true)]
        public string BusinessName { get; set; }

        /// <summary>
        /// 子订单商品
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }

}
