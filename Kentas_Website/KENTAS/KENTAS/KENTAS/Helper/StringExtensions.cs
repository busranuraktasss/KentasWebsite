using System;
using System.Text;

namespace KENTAS.Helper
{
    public static class StringExtensions
    {
        public static string ToBase64Encode(this string Encode)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(Encode);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string ToBase64Decode(this string Decode)
        {
            var base64EncodedBytes = Convert.FromBase64String(Decode);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}