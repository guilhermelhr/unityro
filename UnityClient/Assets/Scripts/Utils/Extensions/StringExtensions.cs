using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringExtensions {

    public static string KoreanTo1252(this string str) => Encoding.GetEncoding(1252).GetString(Encoding.GetEncoding(949).GetBytes(str));

}
