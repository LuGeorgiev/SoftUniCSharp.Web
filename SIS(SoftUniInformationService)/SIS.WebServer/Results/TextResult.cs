namespace SIS.WebServer.Results
{
    using System.Net;
    using System.Text;
    using Http.Responses;
    using SIS.Http.Headers;

    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpStatusCode statusCode) 
            : base(statusCode)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/plain; charset=utf-8"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
