
namespace SIS.Http.Responses.Contracts
{
    using System.Net;
    using Headers.Contracts;
    using Headers;
    using SIS.Http.Cookies;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }
        
        IHttpHeaderCollection Headers { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        byte[] GetBytes();

        void AddCookie(HttpCookie cookie);
    }
}
