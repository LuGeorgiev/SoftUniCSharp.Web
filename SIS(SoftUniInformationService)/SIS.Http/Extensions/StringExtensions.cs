using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Http.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string @string)
        {
            if (!string.IsNullOrEmpty(@string))
            {
                return Char.ToUpper(@string[0]) + @string.Substring(1).ToLower();
            }
            throw new ArgumentException($"{nameof(@string)} cannot be null.");
        }
    }
}
