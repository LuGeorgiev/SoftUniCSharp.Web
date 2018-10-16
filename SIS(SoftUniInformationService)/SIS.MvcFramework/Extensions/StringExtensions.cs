using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SIS.MvcFramework.Extensions
{
    public static class StringExtensions
    {
        public static string UrlDecode(this string input)
        {
            return WebUtility.UrlDecode(input);
        }

        public static decimal ToDecimalOrDefault(this string input)
        {
            if (decimal.TryParse(input, out var result))
            {
                return result;
            }
            return default(decimal);
        }
    }
}
