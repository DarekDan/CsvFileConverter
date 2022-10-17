namespace Fiserv;

public static class StringExtensions
{
    public static string Cleanse(this string s)
    {
        return s.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ');
    }
}