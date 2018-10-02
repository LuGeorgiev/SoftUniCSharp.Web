﻿namespace SIS.Http.Requests.Contracts
{
    using Enums;
    using Headers.Contracts;
    using SIS.Http.Cookies.Contracts;
    using SIS.Http.Session.Contracts;
    using System.Collections.Generic;

    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }

        IHttpCookieCollection Cookies { get; }

        IHttpSession Session { get; set; }
    }
}
