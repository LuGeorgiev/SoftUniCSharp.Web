namespace SIS.WebServer.Results
{
    using SIS.Http.Enums;
    using SIS.Http.Headers;
    using SIS.Http.Responses;
    using System;
    using System.Net;
    using System.Text;

    public class BadRequestResult : HttpResponse
    {
        private const string Default_Error_Heading = "<h1>Error occurred, see details:</h1>";

        public BadRequestResult(string content, HttpResponseStatusCode statusCode)
            : base(statusCode)
        {
            content = Default_Error_Heading + Environment.NewLine + content;
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        //TODO
        //Can be substituted by in code:
        //this.PrepareHtmlResult(content)
        //this.Response.StstusCode = httpResponsestatust.BadReq
        //return Response
    }
}
