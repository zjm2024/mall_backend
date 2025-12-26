using Dm.util;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using shopmallService.Interfaces;
using SqlSugar;
using System.Text;

namespace shopmallService.Services
{
    public  class CartService : ICartService
    {
        private readonly SqlSugarHelper _dbHelper;
        public CartService(SqlSugarHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }



    }
}
