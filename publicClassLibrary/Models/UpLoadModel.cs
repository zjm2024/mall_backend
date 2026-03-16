using publicClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Models
{
    public class UpLoadModel: IUploadConfig
    {
        /// <summary>文件存放绝对路径</summary>
        public string UploadPath { get; set; } = "";

        /// <summary>
        /// 上传URL
        /// </summary>
        public string UploadUrl { get; set; } = "";

     
    }
}
