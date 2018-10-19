using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Api;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.Framework.Routes
{
    public class ResourceRouter : IHttpHandler
    {
        private const string RootDirectory = "../../../";

        public IHttpResponse Handle(IHttpRequest request)
        {
            var path = request.Path;

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
