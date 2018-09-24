using System;
using System.Net;

namespace URLDecode
{
    class StartUp
    {
        static void Main(string[] args)
        {
            string input = "";
            while ((input=Console.ReadLine()).ToLower()!="end")
            {
                var result = ReturnEncodedURL(input);
                Console.WriteLine(result);
            }
        }

        private static string ReturnEncodedURL(string decodedUrl)
        {
            var result = WebUtility.UrlDecode(decodedUrl);
            return result;
        }
    }
}
