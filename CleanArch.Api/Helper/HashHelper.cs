using System.Text;
//using XSystem.Security.Cryptography;

namespace CleanArch.Api.Helper
{
    public class HashHelper
    {
        public static string GetMD5Hash(string plainText)
        {
            try
            {

           
            byte[] bytes = Encoding.ASCII.GetBytes(plainText);
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                 
                byte[] hashBytes = md5.ComputeHash(bytes);

                return Convert.ToHexString(hashBytes);
            }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
