
namespace SIS.Http.Responses.Contracts
{
    using System.Net;
    using Headers.Contracts;
    using Headers;
    using SIS.Http.Cookies;
    using SIS.Http.Cookies.Contracts;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }
        
        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        byte[] GetBytes();

        void AddCookie(HttpCookie cookie);
    }
}
