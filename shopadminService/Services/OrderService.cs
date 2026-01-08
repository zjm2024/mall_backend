using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;
using System.Text;

namespace shopadminService.Services
{
    public  class OrderService : BaseService, IOrderService
    {
        private readonly SqlSugarHelper _dbHelper;
        public OrderService(SqlSugarHelper dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
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

    }
}
