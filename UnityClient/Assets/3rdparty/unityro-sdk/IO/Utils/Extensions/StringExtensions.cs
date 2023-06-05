using System.Text;

namespace ROIO.Utils.Extensions
{
    public static class StringExtensions
    {

        public static string KoreanTo1252(this string str) => Encoding.GetEncoding(1252).GetString(Encoding.GetEncoding(949).GetBytes(str));

    }
}