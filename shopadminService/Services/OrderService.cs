using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
using System.Security.AccessControl;
using System.Text;

namespace shopadminService.Services
{
    public  class OrderService : BaseService, IOrderService
    {
        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public OrderService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
        }

        public ResultObject getOrdersPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? orderStatus)
        {
            try
            {
                //加排序
                List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "CreateTime", OrderByType = OrderByType.Desc });

                //加查询条件



                var conModels = new List<IConditionalModel>();


                conModels.Add(new ConditionalModel { FieldName = "AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });


                if (orderStatus != null)
                {
                    conModels.add(new ConditionalModel { FieldName = "OrderStatus", ConditionalType = ConditionalType.Equal, FieldValue = orderStatus.toString() });
                }

                if (searchKey != null)
                {
                    conModels.Add(new ConditionalCollections()
                    {
                        ConditionalList = new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>()
                    {

                       new KeyValuePair<WhereType, ConditionalModel>(
                       WhereType.And,
                       new ConditionalModel(){FieldName ="OrderNo",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName ="ShippingNo",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),


                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName="ReceiverName",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName="ReceiverPhone",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()})



                    }
                    });
                }

                var totalCount = 0;
                var outobj = GetPageList<Orders>(pageIndex, pageSize, out totalCount, conModels, orderbyList);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }

  
        }

        public ResultObject updateOrders(Orders oV0, string[] updateColums = null)
        {
            //判断订单校验
            var orderId = oV0.OrderId;
            if (orderId == 0)
            {
                return new ResultObject() { Flag = 0, Message = "非法数据!", Result = null };
            }

            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;

                oV0.UpdateTime = DateTime.Now;

                Array.Resize(ref updateColums, updateColums.Length + 1);
                updateColums[updateColums.Length - 1] = "updateTime";

                bool isSuccess = Update<Orders>(oV0, updateColums);


                if (isSuccess)
                    resultobj = new ResultObject() { Flag = 1, Message = "更新成功!", Result = null };
                else
                    resultobj = new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };


                _db.Ado.CommitTran();

                return resultobj;
            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "操作失败!", Result = null };
            }
        }

        public ResultObject getOrdersById(int id)
        {
            try
            {
                var outobj = GetById<Orders>(id);

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public ResultObject getOrdersSubsPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? orderStatus)
        {
            try
            {
                //加排序
                List<OrderByModel> orderbyList = OrderByModel.Create(
               new OrderByModel() { FieldName = "CreateTime", OrderByType = OrderByType.Desc });

                //加查询条件



                var conModels = new List<IConditionalModel>();


                conModels.Add(new ConditionalModel { FieldName = "AppType", ConditionalType = ConditionalType.Equal, FieldValue = appType.toString() });


                if (orderStatus != null)
                {
                    conModels.add(new ConditionalModel { FieldName = "OrderStatus", ConditionalType = ConditionalType.Equal, FieldValue = orderStatus.toString() });
                }

                if (searchKey != null)
                {
                    conModels.Add(new ConditionalCollections()
                    {
                        ConditionalList = new List<KeyValuePair<WhereType, SqlSugar.ConditionalModel>>()
                    {

                       new KeyValuePair<WhereType, ConditionalModel>(
                       WhereType.And,
                       new ConditionalModel(){FieldName ="SubOrderNo",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName ="ShippingNo",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),


                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName="ReceiverName",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()}),

                       new KeyValuePair<WhereType, ConditionalModel> (
                       WhereType.Or,
                       new ConditionalModel() {FieldName="ReceiverPhone",ConditionalType=ConditionalType.Like,FieldValue=searchKey.toString()})



                    }
                    });
                }



                var totalCount = 0;
                var outobj = GetPageList<OrdersSubs>(pageIndex, pageSize, out totalCount, conModels, orderbyList);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj, Count = totalCount };

            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }




     
        }

        public ResultObject getOrdersSubsById(int id)
        {
            try
            {

                var outobj = _db.Queryable<OrdersSubs>()

                .LeftJoin<Business>((s, b) => s.BusinessId == b.BusinessId)
                .Where(s => s.SubOrderId == id)
                .Select((s, b) => new OrdersSubs
                {
                    SubOrderId = s.SubOrderId,
                    SubOrderNo = s.SubOrderNo,
                    OrderId = s.OrderId,
                    OrderNo = s.OrderNo,
                    AppType = s.AppType,
                    PersonalId = s.PersonalId,
                    BusinessId = s.BusinessId,
                    OrderStatus = s.OrderStatus,
                    SubTotalCount = s.SubTotalCount,
                    SubTotalAmount = s.SubTotalAmount,
                    SubShippingFee = s.SubShippingFee,
                    SubDiscountAmount = s.SubDiscountAmount,
                    SubPayAmount = s.SubPayAmount,
                    ShippingNo = s.ShippingNo,
                    ShippingTime = s.ShippingTime,
                    CompleteTime = s.CompleteTime,
                    ReceiverName = s.ReceiverName,
                    ReceiverPhone = s.ReceiverPhone,
                    ReceiverAddress = s.ReceiverAddress,
                    CreateTime = s.CreateTime,
                    UpdateTime = s.UpdateTime,

                    BusinessName = b.BusinessName

                })
                .ToList();



                _db.ThenMapper(outobj, item =>
                {
                    item.OrderItems = _db.Queryable<OrderItems>()
                     .Select((o) => new OrderItems
                     {
                         OrderItemId = o.OrderItemId,
                         OrderId = o.OrderItemId,
                         OrderNo = o.OrderNo,
                         SubOrderId = o.SubOrderId,
                         SubOrderNo = o.SubOrderNo,
                         AppType = o.AppType,
                         PersonalId = o.PersonalId,
                         BusinessId = o.BusinessId,
                         ProductId = o.ProductId,
                         SpecId = o.SpecId,
                         ProductName = o.ProductName,
                         Spec1Name = o.Spec1Name,
                         Spec1Value = o.Spec1Value,
                         Spec2Name = o.Spec2Name,
                         Spec2Value = o.Spec2Value,
                         Spec3Name = o.Spec3Name,
                         Spec3Value = o.Spec3Value,
                         Quantity = o.Quantity,
                         OriginalPrice = o.OriginalPrice,
                         UnitPrice = o.UnitPrice,
                         TotalAmount = o.TotalAmount,
                         PayAmount = o.PayAmount,
                         ActivityType = o.ActivityType,
                         ActivityId = o.ActivityId,
                         RefundStatus = o.RefundStatus,
                         RefundAmount = o.RefundAmount,
                         CreateTime = o.CreateTime,
                         UpdateTime = o.UpdateTime,


                     })
                     .SetContext(o => o.SubOrderId, () => item.SubOrderId, item).ToList();
                });



                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj[0] };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }

        }

    }
}
