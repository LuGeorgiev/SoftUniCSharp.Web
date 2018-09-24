using System;
using System.Net;
using System.Text;

namespace ValidateURL
{
    class StartValidating
    {
        static void Main(string[] args)
        {
            var input = "";
            while ((input = Console.ReadLine()).ToLower()!="end")
            {
                string result = ParseUrl(input);
                Console.WriteLine(result);
            }
        }

        private static string ParseUrl(string url)
        {
            const string InvalidUrl = "Invalid URL";
            bool isUrlValid = true;
            var sb = new StringBuilder();

            url = WebUtility.UrlDecode(url);          

            if (url.IndexOf("http://")==-1
                &&url.IndexOf("https://")==-1)
            {
                return InvalidUrl;
            }

            var uri = new Uri(url);


            var uriScheme = uri.Scheme;
            if (uriScheme==null)
            {
                isUrlValid = false;
            }
            else
            {
                sb.AppendLine($"Protocol: {uriScheme}");
            }

            var uriHost = uri.Host;
            if (uriHost == null)
            {
                isUrlValid = false;
            }
            else
            {
                sb.AppendLine($"Host: {uriHost}");
            }

            var uriPort = uri.Port;
            if (uriPort==-1)
            {
                isUrlValid = false;
            }
            else
            {
                if ((uriPort==80 && uriScheme!="http")
                    || uriPort == 443 && uriScheme != "https")
                {
                    isUrlValid = false;
                }
                else
                {
                    sb.AppendLine($"Port: {uriPort}");
                }
            }

            var uriPath = uri.LocalPath;
            if (uriPath==null)
            {
                isUrlValid = false;
            }
            else
            {                
                sb.AppendLine($"Path: {uriPath}");
            }

            var uriQuery = uri.Query;
            if (!string.IsNullOrEmpty(uriQuery))
            {
                sb.AppendLine($"Query: {uriQuery}");
            }

            var uriFragment = uri.Fragment;
            if (!string.IsNullOrEmpty(uriFragment))
            {
                sb.AppendLine($"Fragment: {uriFragment}");
            }


            if (!isUrlValid)
            {
                return InvalidUrl;
            }
            return sb.ToString().TrimEnd();
        }
    }
}
