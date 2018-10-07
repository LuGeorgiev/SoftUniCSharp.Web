namespace SIS.WebServer.Results
{
    using SIS.Http.Headers;
    using SIS.Http.Responses;
    using System;
    using System.Net;
    using System.Text;

    public class BadRequestResult : HttpResponse
    {
        private static string DefaultErrorHandling = "<h1> Error occured, see details</h1>";

        public BadRequestResult(string content, HttpStatusCode statusCode) 
            : base(statusCode)
        {
            content = DefaultErrorHandling + Environment.NewLine + content;
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
