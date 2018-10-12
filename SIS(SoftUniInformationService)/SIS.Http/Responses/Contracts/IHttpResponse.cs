
namespace SIS.Http.Responses.Contracts
{
    using System.Net;
    using Headers.Contracts;
    using Headers;
    using SIS.Http.Cookies;
    using SIS.Http.Cookies.Contracts;
    using SIS.Http.Enums;

    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; set; }

        IHttpHeadersCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        void AddCookie(HttpCookie cookie);

        byte[] GetBytes();
    }
}
