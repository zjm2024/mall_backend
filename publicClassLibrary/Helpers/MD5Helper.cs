using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Helpers
{
    public static class MD5Helper
    {
        public static string IniPassword = "654321";  //初始密码为654321
        public static string GetMD5(string input)
        {
            byte[] result = Encoding.Default.GetBytes(input);
            using (MD5 md5 = MD5.Create())  
            {
                byte[] output = md5.ComputeHash(result);
                return BitConverter.ToString(output).Replace("-", "").ToLower();
            }

        }


    }
}
