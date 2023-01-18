using System.Globalization;

namespace Fiserv;

public static class StringExtensions
{
    public static string Cleanse(this string s)
    {
        return s.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ');
    }

    public static string ConvertWhenHex(this string s)
    {
        return s.StartsWith("0x") ? Convert.ToChar(Convert.ToInt16(s,16)).ToString() : s;
    }
}