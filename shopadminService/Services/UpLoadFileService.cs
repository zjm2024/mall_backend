using Dm.util;
using NetTaste;
using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Models;
using publicClassLibrary.Services;
using shopadminService.Interfaces;
using SqlSugar;


namespace shopadminService.Services
{
   
    public class UpLoadFileService : BaseService, IUpLoadFileService
    {

        private readonly SqlSugarHelper _dbHelper;
        private readonly ISqlSugarClient _db;
        public UpLoadFileService(SqlSugarHelper dbHelper, ISqlSugarClient db) : base(dbHelper)
        {
            _dbHelper = dbHelper;
            _db = db;
        }


    }
}

