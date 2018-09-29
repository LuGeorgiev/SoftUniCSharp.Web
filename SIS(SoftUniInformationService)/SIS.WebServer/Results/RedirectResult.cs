namespace SIS.WebServer.Results
{
    using Http.Responses;
    using Http.Headers;
    using System.Net;

    public class RedirectResult : HttpResponse
    {
        public RedirectResult(string location)
            :base(HttpStatusCode.Redirect)
        {
            this.Headers.Add(new HttpHeader("Location", location));
        }
    }
}
