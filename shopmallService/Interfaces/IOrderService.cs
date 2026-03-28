using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using System.Text.Json;

namespace shopmallService.Interfaces
{
    public interface IOrderService
    {

        ResultObject getReceiverAddress(int personalId, int appType);

        ResultObject saveReceiverAddress(Address aVo);

        ResultObject delReceiverAddress(int id);

        ResultObject getOrders(int personalId, int appType, string? key);

        ResultObject addOrders(Orders oV0);

        ResultObject updateOrders(Orders oV0, string[] updateColums = null);
        ResultObject getOrdersById(int id, int appType);

        ProductSpecs getProductSpecsById(int id);
    }
}
