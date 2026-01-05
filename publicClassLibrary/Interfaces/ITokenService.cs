using publicClassLibrary.Entitys;

namespace publicClassLibrary.Interfaces
{
    public interface ITokenService
    {
        public bool TokenIsExist(string token);

        
        public void UpdateTokenTime(Tokens tVO);

        public void RemoveToken(Tokens tVO);

    }
}
