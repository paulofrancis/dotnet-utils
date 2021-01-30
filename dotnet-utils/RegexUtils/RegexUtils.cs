using System.Text.RegularExpressions;

namespace dotnet_utils.RegexUtils
{
    public static class RegexUtils
    {
        public static string SanitizeNumber(string number)
        {
            var rgx = new Regex("[^0-9]");
            return rgx.Replace(number, "");
        }
    }
}
