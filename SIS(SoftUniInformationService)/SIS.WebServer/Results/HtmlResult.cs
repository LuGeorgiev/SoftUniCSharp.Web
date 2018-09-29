﻿namespace SIS.WebServer.Results
{
    using System.Net;
    using System.Text;
    using Http.Responses;
    using SIS.Http.Headers;

    public class HtmlResult : HttpResponse
    {
        public HtmlResult( string content, HttpStatusCode statusCode) 
            : base(statusCode)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
