using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopadminService.Interfaces;
using System.Text.Json;


namespace shopadminService.Controllers
{
    [Anonymous]
    [ApiController]
    [Route("shopadminApi/Category/[action]")]
    public class CategoryController : ControllerBase
    {

        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryservice;
        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryservice)
        {
            _logger = logger;
            _categoryservice = categoryservice;
        }

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        [HttpGet]
        public ResultObject getCategoriesList(int appType, string? categoryName, int? status)
        {
            var list = _categoryservice.getCategoriesList(appType, categoryName, status);
            return new ResultObject() { Flag = 1, Message = "获取成功!", Result = list, Count = list.Count, Subsidiary = 1 };
        }

        /// <summary>
        /// 更新或插入分类
        /// </summary>
        [HttpPost]
        public ResultObject updateCategories([FromBody] JsonElement formData)
        {
            JsonElement jValue;
            string json = ((!formData.TryGetProperty("data", out jValue)) ? "" : jValue.GetRawText());
            var entity = JsonConvert.DeserializeObject(json, typeof(Categories));
            if (entity == null)
            {
                return new ResultObject() { Flag = 0, Message = "参数为空!", Result = null };
            }
            return _categoryservice.updateCategories((Categories)entity);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        [HttpGet]
        public ResultObject deleteCategories(int id)
        { 
            return _categoryservice.deleteCategories(id);
        }
    }
}
