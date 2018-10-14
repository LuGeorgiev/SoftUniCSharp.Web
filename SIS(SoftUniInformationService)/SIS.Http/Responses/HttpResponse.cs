namespace SIS.Http.Responses
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Contracts;
    using Common;
    using Cookies;
    using Cookies.Contracts;
    using Extensions;
    using Headers;
    using Headers.Contracts;
    using SIS.Http.Enums;

    public class HttpResponse : IHttpResponse
    {
        private const string Cookie_Response_Header = "Set-Cookie";

        public HttpResponse()
        {
            this.Headers = new HttpHeadersCollection();
            this.Content = new byte[0];
            this.Cookies = new HttpCookieCollection();
        }

        public HttpResponse(HttpResponseStatusCode statusCode)
            :this()
        {            
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeadersCollection Headers { get; private set; }

        public byte[] Content { get; set; }

        public IHttpCookieCollection Cookies { get; }

        public void AddCookie(HttpCookie cookie)
        {
            this.Cookies.Add(cookie);
        }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.Add(header);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}")
                .AppendLine($"{this.Headers}");

            if (this.Cookies.HasCookies())
            {
                result.AppendLine($"{Cookie_Response_Header}: {this.Cookies}");
            }

            result.AppendLine();

            return result.ToString();
        }
    }
}
