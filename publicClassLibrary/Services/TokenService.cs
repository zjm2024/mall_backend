using publicClassLibrary.Entitys;
using publicClassLibrary.Helpers;
using publicClassLibrary.Interfaces;
using System.Text;

namespace publicClassLibrary.Services
{
    public class TokenService: ITokenService
    {
        private  int TokenTimeout = 3600 * 24 * 14;

        private readonly SqlSugarHelper _dbHelper;
        public TokenService(SqlSugarHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public bool TokenIsExist(string token)
        {
            int userId;
            string cacheKey;
            TokenSplit(token, out userId, out cacheKey);

            var list = _dbHelper.GetList<Tokens>(it => it.Token == token && it.UserId == userId);


            if (list.Count > 0)
            {
                var tVO = ((Tokens)list[0]);
                var timeout = DateTime.Parse(tVO.Timeout.ToString());
                if (timeout > DateTime.Now)
                {
                    //如果已经存在，更新timout
                    tVO.Timeout = DateTime.Now.AddSeconds(TokenTimeout);
                   
                    UpdateTokenTime(tVO);
                    return true;
                }
                else
                {
                    RemoveToken(tVO);
                    return false;
                }
            }
            return false;
        }


        public void UpdateTokenTime(Tokens tVO)
        {
            _dbHelper.Update<Tokens>(tVO, it => new { it.Timeout });
        }
        public void RemoveToken(Tokens tVO)
        {
            int tokenId = tVO.TokenId;
            _dbHelper.Delete<Tokens>(it => it.TokenId == tokenId);
        }
        

        private void TokenSplit(string token, out int userId, out string cacheKey)
        {
            string[] parts = token.Split('.');
            if (parts.Length > 1)
            {
                string guid = parts[0];
                string base64Userid = parts[1];
                byte[] decodedBytes = Convert.FromBase64String(base64Userid);
                string userid = Encoding.UTF8.GetString(decodedBytes);
                userId = Convert.ToInt32(userid);
                cacheKey = $"PASSPORT.TOKEN.{userId}";
            }
            else
            {
                string guid = parts[0];
                userId = 0;
                cacheKey = $"PASSPORT.TOKEN.{userId}";
            }
        }


    }
}
