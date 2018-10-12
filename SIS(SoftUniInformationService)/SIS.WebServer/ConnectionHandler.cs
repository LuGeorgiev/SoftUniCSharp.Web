namespace SIS.WebServer
{
    using Http.Requests;
    using Http.Requests.Contracts;
    using Http.Responses;
    using Http.Responses.Contracts;
    using Routing;
    using SIS.Http.Cookies;
    using SIS.Http.Session;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Http.Common;
    using System.Linq;
    using System.Reflection;
    using SIS.WebServer.Results;
    using System.IO;
    using SIS.Http.Exceptions;
    using SIS.Http.Enums;

    public class ConnectionHandler
    {
        private readonly Socket client;
        private readonly ServerRoutingTable serverRoutingTable;

        private const string RootDirectory = "../../../";

        public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        {
            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);
                if (numberOfBytesRead==0)
                {
                   break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead<1023)
                {
                    break;
                }
            }

            if (result.Length==0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }       

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            if (IsResourceRequest(httpRequest))
            {
                return this.HandleRequestResponse(httpRequest.Path);
            }
           
            if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
            || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }
                      

            return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
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

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;
            if (requestPath.Contains("."))
            {
                var pathExtension = requestPath.Substring(requestPath.LastIndexOf('.'));
                bool isValid= GlobalConstants.ResourceExtensions.Contains(pathExtension);

                return isValid;
            }
            //httpRequest.Path -> .js, .css -> Resource/css/?! -> bootestrap.min
            return false;
        }

        private string SetRequestSession(IHttpRequest request)
        {
            string sessionId = null;

            if (request.Cookies.ContainsCookie(HttpSessionStorage.Session_Cookie_Key))
            {
                var cookie = request.Cookies.GetCookie(HttpSessionStorage.Session_Cookie_Key);
                sessionId = cookie.Value;
                request.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                request.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private void SetResponseSession(IHttpResponse response, string sessionId)
        {
            if (sessionId != null)
            {
                response.AddCookie(new HttpCookie(HttpSessionStorage.Session_Cookie_Key, $"{sessionId}; HttpOnly"));
            }
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            if (httpRequest != null)
            {
                string sessionId = this.SetRequestSession(httpRequest);

                var httpResponse = this.HandleRequest(httpRequest);

                this.SetResponseSession(httpResponse, sessionId);

                await this.PrepareResponse(httpResponse);
            }

            this.client.Shutdown(SocketShutdown.Both);
        }
    }

}
