using SIS.Http.Enums;
using SIS.Http.Headers;
using SIS.Http.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SIS.WebServer.Results
{
    public class InlineResourceResult : HttpResponse
    {
        public InlineResourceResult(byte[] content, HttpResponseStatusCode statusCode) 
            : base(statusCode)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Content = content;
        }
    }
}
