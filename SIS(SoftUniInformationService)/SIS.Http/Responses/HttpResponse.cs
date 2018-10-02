namespace SIS.Http.Responses
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Contracts;
    using SIS.Http.Common;
    using SIS.Http.Cookies;
    using SIS.Http.Cookies.Contracts;
    using SIS.Http.Extensions;
    using SIS.Http.Headers;
    using SIS.Http.Headers.Contracts;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
        }

        public HttpResponse(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.Content = new byte[0];
        }

        public HttpStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            this.Cookies.Add(cookie);
        }

        public byte[] GetBytes()
             => Encoding.UTF8
                .GetBytes(this.ToString())
                .Concat(this.Content)
                .ToArray();
        

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine($"{GlobalConstants.HttpOnProtocolFrame} {this.StatusCode.GetResponseLine()}")
                .AppendLine($"{this.Headers}");

            if (this.Cookies.HasCookies())
            {
                result.Append($"Set-Cookie: {this.Cookies}")
                    .Append(Environment.NewLine);
            }

            result.Append(Environment.NewLine);

            return result.ToString();
        }
    }
}
