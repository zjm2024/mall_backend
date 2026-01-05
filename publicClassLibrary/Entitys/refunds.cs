using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    // 11. 退款申请表 (mall_refunds)
    // ----------------------------
    [SugarTable("mall_refunds")]
    public class Refunds
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int RefundId { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "OrderId", ColumnDescription = "订单ID")]
        public int OrderId { get; set; }

        [SugarColumn(ColumnName = "OrderItemId", ColumnDescription = "订单明细ID(单商品退款)")]
        public int OrderItemId { get; set; }

        [SugarColumn(ColumnName = "PersonalID", ColumnDescription = "申请人")]
        public int PersonalID { get; set; }

        [SugarColumn(ColumnName = "BusinessId", ColumnDescription = "商家ID")]
        public int BusinessId { get; set; }

        [SugarColumn(ColumnName = "RefundType", ColumnDescription = "退款类型:1-仅退款 2-退款退货")]
        public int RefundType { get; set; }

        [SugarColumn(ColumnName = "RefundReason", ColumnDescription = "退款原因", Length = 255)]
        public string RefundReason { get; set; }

        [SugarColumn(ColumnName = "RefundAmount", ColumnDescription = "申请退款金额")]
        public decimal RefundAmount { get; set; }

        [SugarColumn(ColumnName = "RealRefundAmount", ColumnDescription = "实际退款金额")]
        public decimal RealRefundAmount { get; set; }

        [SugarColumn(ColumnName = "Status", ColumnDescription = "状态:1-申请中 2-已同意 3-已拒绝 4-已退款 5-已关闭")]
        public int Status { get; set; }

        [SugarColumn(ColumnName = "AdminRemark", ColumnDescription = "管理员备注", Length = 255)]
        public string AdminRemark { get; set; }

        [SugarColumn(ColumnName = "UserRemark", ColumnDescription = "用户备注", Length = 255)]
        public string UserRemark { get; set; }

        [SugarColumn(ColumnName = "ReturnShippingNo", ColumnDescription = "退货物流单号", Length = 100)]
        public string ReturnShippingNo { get; set; }

        [SugarColumn(ColumnName = "ReturnShippingCompany", ColumnDescription = "退货快递公司", Length = 50)]
        public string ReturnShippingCompany { get; set; }

        [SugarColumn(ColumnName = "EvidenceImages", ColumnDescription = "凭证图片(JSON)")]
        public string EvidenceImages { get; set; }

        [SugarColumn(ColumnName = "TransactionId", ColumnDescription = "退款交易号", Length = 255)]
        public string TransactionId { get; set; }

        [SugarColumn(ColumnName = "ApplyTime", ColumnDescription = "申请时间")]
        public DateTime ApplyTime { get; set; }

        [SugarColumn(ColumnName = "HandleTime", ColumnDescription = "处理时间")]
        public DateTime HandleTime { get; set; }

        [SugarColumn(ColumnName = "RefundTime", ColumnDescription = "退款到账时间")]
        public DateTime RefundTime { get; set; }

        [SugarColumn(ColumnName = "CloseTime", ColumnDescription = "关闭时间")]
        public DateTime CloseTime { get; set; }

        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime UpdateTime { get; set; }
    }

}
