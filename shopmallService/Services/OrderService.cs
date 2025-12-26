using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using shopmallService.Interfaces;
using SqlSugar;
using System.Text;

namespace shopmallService.Services
{
    public  class OrderService : IOrderService
    {
        private readonly SqlSugarHelper _dbHelper;
        public OrderService(SqlSugarHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

 

    }
}
