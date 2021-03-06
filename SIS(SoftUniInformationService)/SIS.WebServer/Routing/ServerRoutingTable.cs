﻿namespace SIS.WebServer.Routing
{
    using Http.Enums;
    using Http.Requests.Contracts;
    using Http.Responses.Contracts;
    using System;
    using System.Collections.Generic;

    public class ServerRoutingTable
    {
        public ServerRoutingTable()
        {
            this.Routes = new Dictionary<HttpRequestMethod, Dictionary<string, Func<IHttpRequest, IHttpResponse>>>
            {
                [HttpRequestMethod.GET]=new Dictionary<string,Func<IHttpRequest,IHttpResponse>>(),
                [HttpRequestMethod.POST]=new Dictionary<string,Func<IHttpRequest,IHttpResponse>>(),
                [HttpRequestMethod.PUT]=new Dictionary<string,Func<IHttpRequest,IHttpResponse>>(),
                [HttpRequestMethod.DELETE]=new Dictionary<string,Func<IHttpRequest,IHttpResponse>>(),
            };
        }

        public Dictionary<HttpRequestMethod,Dictionary<string, Func<IHttpRequest, IHttpResponse>>> Routes { get; }

        public void  Add(HttpRequestMethod method, string path, Func<IHttpRequest, IHttpResponse> func)
        {
            this.Routes[method].Add(path, func);
        }
    }
}
