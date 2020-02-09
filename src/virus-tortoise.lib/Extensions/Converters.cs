using System.Linq;

namespace virus_tortoise.lib.Extensions
{
    public static class Converters
    {
        public static int ToInt32(this byte[] data, int startIndex = 0)
        {
            data = data.Skip(startIndex).Take(4).Reverse().ToArray();

            int result = 0;

            for (int i = 0; i < 3; i++)
            {
                result = data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
            }
         
            return result;            
        }
    }
}