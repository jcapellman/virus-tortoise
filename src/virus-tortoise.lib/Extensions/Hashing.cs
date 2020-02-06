using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace virus_tortoise.lib.Extensions
{
    public static class Hashing
    {
        public static string ToSHA256(this byte[] data)
        {
            using (var sha = new SHA256Managed())
            {
                return string.Concat(sha.ComputeHash(data).Select(item => 
                    item.ToString("x2", CultureInfo.InvariantCulture))).ToUpper(CultureInfo.InvariantCulture);
            }
        }
    }
}