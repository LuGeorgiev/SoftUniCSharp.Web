using System.IO;
using System.Linq;
using SIS.Http.Common;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Api;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;

namespace SIS.Framework
{
    public class HttpHandler : IHttpHandler
    {
        private const string RootDirectory = "../../../";
        private ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable routingTable)
        {
            this.serverRoutingTable = routingTable;
        }

        public IHttpResponse Handle(IHttpRequest request)
        {           

            if (!this.serverRoutingTable.Routes.ContainsKey(request.RequestMethod)
            || !this.serverRoutingTable.Routes[request.RequestMethod].ContainsKey(request.Path))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Routes[request.RequestMethod][request.Path].Invoke(request);
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;
            if (requestPath.Contains("."))
            {
                var pathExtension = requestPath.Substring(requestPath.LastIndexOf('.'));
                bool isValid = GlobalConstants.ResourceExtensions.Contains(pathExtension);

                return isValid;
            }

            //httpRequest.Path -> .js, .css -> Resource/css/?! -> bootestrap.min
            return false;
        }

        private IHttpResponse HandleRequestResponse(string path)
        {
            var indexOfStartOfExtension = path.LastIndexOf('.');
            var indexOfStartOfNameOfResource = path.LastIndexOf('/');
            var pathExtension = path.Substring(path.LastIndexOf('.'));

            var resourceName = path.Substring(indexOfStartOfNameOfResource);

            var resourcePath = RootDirectory
                + "Resources"
                + $"/{pathExtension.Substring(1)}"  // to skip the . and extarct css
                + resourceName;

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            var fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        }

    }
}
