using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using publicClassLibrary.Entitys;
using publicClassLibrary.Models;
using publicClassLibrary.TokenMange;
using shopadminService.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;


namespace shopadminService.Controllers
{
    [Anonymous]
    [ApiController]
    [Route("shopadminApi/UploadFile/[action]")]
    public class UploadFileController : ControllerBase
    {

        private readonly ILogger<UploadFileController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUpLoadFileService _uploadfileservice;

        public UploadFileController(ILogger<UploadFileController> logger, IHttpContextAccessor httpContextAccessor,IUpLoadFileService uploadfileservice)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _uploadfileservice = uploadfileservice;
        }


        /// <summary>
        /// 上传图片
        /// </summary>
  
        [HttpPost]
        public async Task<ResultObject> uploadImage([FromForm] int appType, [FromForm] int businessId, IFormFile file)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext?.Request;
                string url = request.Scheme+"://" +request.Host.ToString();
                string root=  Directory.GetCurrentDirectory()+"/wwwroot";
                string folder = "/UploadFolder/Image/" + businessId.ToString()+"/"+ DateTime.Now.ToString("yyyyMM") + "/";

                string ext=Path.GetExtension(file.FileName).ToLowerInvariant();
                string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + ext;

                //可以修改为网络路径
                string localPath = root + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;


                await  using var stream = file.OpenReadStream();
                await using var fileStream = new FileStream(PhysicalPath, FileMode.OpenOrCreate);
                await file.CopyToAsync(fileStream);

                var res = new { src = url + folder + newFileName };
        
                var result = new ResultObject() { Flag = 1, Message = "上传成功", Result = res };
                return result;



            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "上传失败"+ ex.ToString(), Result = null };
            }

        }



        /// <summary>
        /// 上传文件
        /// </summary>
 
        [HttpPost]
        public async Task<ResultObject> uploadFile([FromForm] int appType, [FromForm] int businessId, IFormFile file)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext?.Request;
                string url = request.Scheme + "://" + request.Host.ToString();
                string root = Directory.GetCurrentDirectory() + "/wwwroot";
                string folder = "/UploadFolder/Attached/" + businessId.ToString() + "/" + DateTime.Now.ToString("yyyyMM") + "/";

                string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + ext;

                //可以修改为网络路径
                string localPath = root + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;


                await using var stream = file.OpenReadStream();
                await using var fileStream = new FileStream(PhysicalPath, FileMode.OpenOrCreate);
                await file.CopyToAsync(fileStream);

                var res = new { src = url + folder + newFileName };

                var result = new ResultObject() { Flag = 1, Message = "上传成功", Result = res };
                return result;



            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "上传失败" + ex.ToString(), Result = null };
            }
        }

        /// <summary>
        /// 上传音频   PC不支持音频
        /// </summary>

        [HttpPost]
        public async Task<ResultObject> uploadAudio([FromForm] int appType, [FromForm] int businessId, IFormFile file)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext?.Request;
                string url = request.Scheme + "://" + request.Host.ToString();
                string root = Directory.GetCurrentDirectory() + "/wwwroot";
                string folder = "/UploadFolder/Audio/" + businessId.ToString() + "/" + DateTime.Now.ToString("yyyyMM") + "/";

                string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                string newFileName = DateTime.Now.ToString("yyyyMMddhhssmm") + ext;

                //可以修改为网络路径
                string localPath = root + folder;
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                string PhysicalPath = localPath + newFileName;


                await using var stream = file.OpenReadStream();
                await using var fileStream = new FileStream(PhysicalPath, FileMode.OpenOrCreate);
                await file.CopyToAsync(fileStream);

                var res = new { src = url + folder + newFileName };

                var result = new ResultObject() { Flag = 1, Message = "上传成功", Result = res };
                return result;



            }
            catch (Exception ex)
            {
                return new ResultObject() { Flag = 0, Message = "上传失败" + ex.ToString(), Result = null };
            }


        }



    }
}

