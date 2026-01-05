using publicClassLibrary.Helpers;
using publicClassLibrary.Services;
using shopmallService.Interfaces;
using SqlSugar;
using System.Text;

namespace shopmallService.Services
{
    public  class OrderService : BaseService, IOrderService
    {
        private readonly SqlSugarHelper _dbHelper;
        public OrderService(SqlSugarHelper dbHelper) : base(dbHelper)
        {
            _dbHelper = dbHelper;
        }

 

    }
}
