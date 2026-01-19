using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
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

        public List<Orders>  getOrdersPageList(int pageIndex, int pageSize, int appType, string? searchKey, int? orderStatus, out int totalCount)
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


            var list = GetPageList<Orders>(pageIndex, pageSize, out totalCount, conModels, orderbyList);
            return list;
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

        public Orders getOrdersById(int id)
        {
            var outobj = GetById<Orders>(id);
            return outobj;
        }

    }
}
