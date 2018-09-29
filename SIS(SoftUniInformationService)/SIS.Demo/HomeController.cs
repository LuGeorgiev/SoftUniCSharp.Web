namespace SIS.Demo
{
    using Http.Responses;
    using SIS.Http.Responses.Contracts;
    using SIS.WebServer.Results;
    using System.Net;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1> Hello, World!</h1>";

            return new HtmlResult(content, HttpStatusCode.OK);
        }
    }
}
