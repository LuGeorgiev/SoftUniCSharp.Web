namespace SIS.WebServer.Results
{
    using Http.Responses;
    using Http.Headers;
    using System.Net;
    using SIS.Http.Enums;

    public class RedirectResult : HttpResponse
    {
        public RedirectResult(string location)
            : base(HttpResponseStatusCode.SeeOther)
        {
            this.Headers.Add(new HttpHeader("Location", location));
        }
    }
}
