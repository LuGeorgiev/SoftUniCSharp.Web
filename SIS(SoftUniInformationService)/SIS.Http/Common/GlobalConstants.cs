namespace SIS.Http.Common
{
    public class GlobalConstants
    {
        public const string HttpOneProtocolFrame = "HTTP/1.1";

        public const string HostHeaderKey = "Host";

        public const string CookieRequestHeaderName = "Cookie";

        public const string HttpNewLine = "\r\n";

        public const string CookieResponseHeaderName = "Set-Cookie";

        public const int NumberOfParametersInReuestKVP = 2;

        public static string[] ResourceExtensions = new string[] {".js", ".css" };
    }
}
