using System.Security.Cryptography;
using System.Text;

namespace TecWi_Web.Shared.Utility
{
    public static class PasswordHashUtitlity
    {
        public static void CreatePaswordHash(string senha, out byte[] senhadHash, out byte[] senhaSalt)
        {
            using (var hmacsha = new HMACSHA512())
            {
                senhaSalt = hmacsha.Key;
                senhadHash = hmacsha.ComputeHash(Encoding.UTF8.GetBytes(senha));
            }
        }
    }
}
