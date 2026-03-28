using Microsoft.AspNetCore.Mvc;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopmallService.Interfaces;
using shopmallService.Services;

namespace shopmallService.Controllers
{
    [ApiController]
    [Route("shopmallApi/DataDict/[action]")]
    public class DataDictController : ControllerBase
    {
   
        private readonly ILogger<DataDictController> _logger;
        private readonly IDataDictService _datadictservice;
        public DataDictController(ILogger<DataDictController> logger, IDataDictService datadictservice)
        {
            _logger = logger;
            _datadictservice = datadictservice;
        }

        [HttpGet, Anonymous]
        public async Task<ResultObject> getDataDictByCode(string code)
        {
            var resultObject = await _datadictservice.getDataDictByCode(code);
            return resultObject;

        }


    }
}
