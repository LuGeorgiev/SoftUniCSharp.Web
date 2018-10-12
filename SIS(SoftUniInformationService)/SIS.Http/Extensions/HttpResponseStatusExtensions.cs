namespace SIS.Http.Extensions
{
    using SIS.Http.Enums;
    using System.Net;

    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode)
            => $"{(int)statusCode} {statusCode}";
    }
}
