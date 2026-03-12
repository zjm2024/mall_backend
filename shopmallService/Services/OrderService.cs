using Dm.util;
using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopmallService.Interfaces;
using SQLitePCL;
using SqlSugar;
using StackExchange.Redis;
using System.Collections;
using System.Text;
using System.Text.Json;

namespace shopmallService.Services
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
        public ResultObject getReceiverAddress(int personalId, int appType)
        {
            try
            {
                List<OrderByModel> orderbyList = OrderByModel.Create(
                 new OrderByModel() { FieldName = "IsDefault", OrderByType = OrderByType.Desc },
                 new OrderByModel() { FieldName = "CreateTime", OrderByType = OrderByType.Desc }
                 );
                var outobj = GetList<Address>(it => it.PersonalID == personalId  && it.AppType== appType, orderbyList);
                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = outobj };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public ResultObject saveReceiverAddress(Address aVo)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;
                if (aVo.AddressId == 0)
                {
                    int addressId = Add<Address>(aVo);
                    aVo.AddressId = addressId;

                    if (addressId > 0)
                        resultobj = new ResultObject() { Flag = 1, Message = "保存成功!", Result = aVo };               
                    else
                        resultobj = new ResultObject() { Flag = 0, Message = "保存失败!", Result = null };
                }
                else
                {
                    aVo.UpdateTime = DateTime.Now;
                    bool isSuccess = Update<Address>(aVo);
                    if (isSuccess)
                        resultobj = new ResultObject() { Flag = 1, Message = "更新成功!", Result = aVo };
                    else
                        resultobj = new ResultObject() { Flag = 0, Message = "更新失败!", Result = null };
                 

                }
                //如果传递的是默认地址 则把其他地址改成不是默认
                if (aVo.IsDefault==1)
                {
                    _db.Updateable<Address>()
                   .SetColumns(it => it.IsDefault == 0)
                   .Where(it => it.AddressId != aVo.AddressId && it.PersonalID== aVo.PersonalID && it.AppType==aVo.AppType)
                   .ExecuteCommand();
                }

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




        public ResultObject delReceiverAddress(int id)
        {
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;
                bool isSuccess = Delete<Address>(id);
                if (isSuccess)
                {
                    resultobj= new ResultObject() { Flag = 1, Message = "删除成功!", Result = id };
                }
                else
                {
                    resultobj= new ResultObject() { Flag = 0, Message = "删除失败!", Result = null };
                }


                _db.Ado.CommitTran();
                return resultobj;

            }
            catch (Exception ex)
            {
                // 如果有任何异常，回滚事务
                _db.Ado.RollbackTran();
                return new ResultObject() { Flag = 0, Message = "删除失败!" + ex.toString(), Result = null };
            }
        }

        public ResultObject getOrders(int personalId, int appType, string? key)
        {
            try
            {
                //查询订单
                List<OrderByModel> orderbyList = OrderByModel.Create(
                new OrderByModel() { FieldName = "CreateTime", OrderByType = OrderByType.Desc }
                 );
            
                var   outobj = _db.Queryable<Orders>().Where(it => it.PersonalId == personalId && it.AppType == appType).OrderBy(orderbyList).ToList();

                _db.ThenMapper(outobj, item => {
                   item.OrdersSubs = _db.Queryable<OrdersSubs>().LeftJoin<Business>((o, b) => o.BusinessId == b.BusinessId)
                    .Select((o, b) => new OrdersSubs
                    {
                        SubOrderId = o.SubOrderId,
                        SubOrderNo = o.SubOrderNo,
                        AppType = o.AppType,
                        OrderId = o.OrderId,
                        OrderNo = o.OrderNo,
                        PersonalId = o.PersonalId,
                        BusinessId = o.BusinessId,
                        SubPayAmount = o.SubPayAmount,
                        SubTotalAmount = o.SubTotalAmount,
                        SubDiscountAmount = o.SubDiscountAmount,
                        SubShippingFee = o.SubShippingFee,
                        CreateTime = o.CreateTime,
                        UpdateTime = o.UpdateTime,
                        BusinessName = b.BusinessName
                    })
                    .SetContext(o => o.OrderId, () => item.OrderId, item).ToList();
                });

                //第二层
                _db.ThenMapper(outobj.SelectMany(it => it.OrdersSubs), it =>
                {
                    it.OrderItems = _db.Queryable<OrderItems>().SetContext(x => x.SubOrderId, () => it.SubOrderId, it).ToList();
                });

                object res = new
                {
                    ordersList = outobj,

                };

                return new ResultObject() { Flag = 1, Message = "获取成功!", Result = res };
            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "获取失败!", Result = ex.ToString() };
            }
        }

        public  ResultObject addOrders(Orders oV0)
        {
            //判断订单校验
            _db.Ado.BeginTran();
            try
            {
                dynamic resultobj;

                int orderId = Add<Orders>(oV0);

                if (orderId > 0)
                {
                    var orderItems = new ArrayList();
                    foreach (OrdersSubs item in oV0.OrdersSubs)
                    {
                        item.OrderId = orderId;
                        item.OrderNo = oV0.OrderNo;
                        //生成子订单号
                        Random ran = new Random();
                        item.SubOrderNo= "SD" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ran.Next(10000, 99999);

                        int suborderId = Add<OrdersSubs>(item);

                        item.OrderItems.ForEach(it =>
                        {
                            it.OrderId = orderId;
                            it.OrderNo = oV0.OrderNo;
                            it.SubOrderId = suborderId;
                            it.SubOrderNo = item.SubOrderNo;
                        });
                        //保存多条记录
                        _db.Storageable(item.OrderItems).ExecuteCommand();
                    }
          
                }

                    if (orderId>0)
                    resultobj = new ResultObject() { Flag = 1, Message = "保存成功!", Result = null };
                else
                    resultobj = new ResultObject() { Flag = 0, Message = "保存失败!", Result = null };


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






        public ResultObject getOrdersById(int id, int appType)
        {
            try
            {
                var outobj = _db.Queryable<Orders>().Where(it => it.OrderId == id && it.AppType == appType).ToList();
                //获取商品明细
                _db.ThenMapper(outobj, item => {
                    item.OrdersSubs= _db.Queryable<OrdersSubs>().LeftJoin<Business>((o, b) => o.BusinessId == b.BusinessId)
                    .Select((o,b)=>   new OrdersSubs{
                        SubOrderId=o.SubOrderId,
                        SubOrderNo=o.SubOrderNo,
                        AppType=o.AppType,
                        OrderId= o.OrderId,
                        OrderNo= o.OrderNo,
                        PersonalId= o.PersonalId,
                        BusinessId= o.BusinessId,
                        SubPayAmount= o.SubPayAmount,
                        SubTotalAmount=o.SubTotalAmount,
                        SubDiscountAmount= o.SubDiscountAmount,
                        SubShippingFee= o.SubShippingFee,
                        CreateTime= o.CreateTime,
                        UpdateTime=o.UpdateTime,
                        BusinessName= b.BusinessName
                    })
                    .SetContext(o => o.OrderId, () => item.OrderId, item).ToList();
                });

                //第二层
                _db.ThenMapper(outobj.SelectMany(it => it.OrdersSubs), it =>
                {
                    it.OrderItems = _db.Queryable<OrderItems>().SetContext(x => x.SubOrderId, () => it.SubOrderId, it).ToList();
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
