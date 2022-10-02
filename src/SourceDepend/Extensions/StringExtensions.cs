using System.Globalization;

namespace SourceDepend.Extensions
{
    internal static class StringExtensions
    {
        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            if (!char.IsUpper(s[0]))
                return s;

            string camelCase = null;
#if !(NETFX_CORE || PORTABLE)
            camelCase = char.ToLower(s[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
#else
      camelCase = char.ToLower(s[0]).ToString();
#endif

            if (s.Length > 1)
                camelCase += s.Substring(1);

            return camelCase;
        }
    }
}
