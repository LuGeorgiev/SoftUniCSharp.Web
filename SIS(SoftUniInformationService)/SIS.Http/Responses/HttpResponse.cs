namespace SIS.Http.Responses
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Contracts;
    using SIS.Http.Common;
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
            this.Content = new byte[0];
        }

        public HttpStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.Add(header);
        }

        public byte[] GetBytes()
             => Encoding.UTF8
                .GetBytes(this.ToString())
                .Concat(this.Content)
                .ToArray();
        

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append($"{GlobalConstants.HttpOnProtocolFrame} {this.StatusCode.GetResponseLine()}")
                .Append(Environment.NewLine)
                .Append(this.Headers)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine);

            return result.ToString();
        }
    }
}
