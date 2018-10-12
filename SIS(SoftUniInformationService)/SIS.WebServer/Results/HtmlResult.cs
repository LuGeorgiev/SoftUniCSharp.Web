﻿namespace SIS.WebServer.Results
{
    using System.Net;
    using System.Text;
    using Http.Responses;
    using SIS.Http.Enums;
    using SIS.Http.Headers;

    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode statusCode)
           : base(statusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
