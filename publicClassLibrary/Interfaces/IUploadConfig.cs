using publicClassLibrary.Entitys;

namespace publicClassLibrary.Interfaces
{
    public interface IUploadConfig
    {
        string UploadPath { get; set; } // 本地保存路径
        string UploadUrl { get; set; }  // 访问地址

    }
}
