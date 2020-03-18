using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContosoUniversity.Infrastructure
{
    public static class StringExtensions
    {
        public static string BreakUpCamelCase(this string value)
        {
            var patterns = new[]
            {
                "([a-z])([A-Z])",
                "([0-9])([a-zA-Z])",
                "([a-zA-Z])([0-9])"
            };
            var output = patterns.Aggregate(value,
                (current, pattern) => Regex.Replace(current, pattern, "$1 $2", RegexOptions.IgnorePatternWhitespace));
            return output.Replace('_', ' ');
        }
    }
}
