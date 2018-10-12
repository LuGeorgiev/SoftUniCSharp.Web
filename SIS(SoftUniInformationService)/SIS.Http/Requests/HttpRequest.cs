namespace SIS.Http.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Common;
    using Cookies;
    using Enums;
    using Exceptions;
    using Extensions;
    using Headers;
    using SIS.Http.Session;
    using SIS.Http.Cookies.Contracts;
    using SIS.Http.Headers.Contracts;
    using SIS.Http.Requests.Contracts;
    using SIS.Http.Session.Contracts;

    public class HttpRequest : IHttpRequest
    {
        private const string Cookie_Header = "Cookie";

        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeadersCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public IHttpCookieCollection Cookies { get; }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeadersCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpSession Session { get; set; }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var requestLine = splitRequestContent[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!this.ValidateRequestLine(requestLine))
            {
                throw new BadRequestException("Invalid request line");
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader(Cookie_Header))
            {
                return;
            }

            var cookies = this.Headers.GetHeader(Cookie_Header).Value;

            var cookiesSplit = cookies.Split("; ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var cookie in cookiesSplit)
            {
                var cookiePair = cookie.Split("=", 2);

                if (cookiePair.Length != 2)
                {
                    throw new BadRequestException("Cookie key value pair is invalid.");
                }

                var cookieName = cookiePair[0];
                var cookieValue = cookiePair[1];

                this.Cookies.Add(new HttpCookie(cookieName, cookieValue));
            }
        }

        private void ParseRequestParameters(string bodyParameters)
        {
            this.ParseQueryParameters(this.Url);

            if (this.RequestMethod == HttpRequestMethod.GET)
            {
                return;
            }

            ParseFormDataParameters(bodyParameters);
        }

        private void ParseFormDataParameters(string bodyParameters)
        {
            var formDataKeyValuePairs = bodyParameters.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var formDataKeyValuePair in formDataKeyValuePairs)
            {
                var keyValuePair = formDataKeyValuePair.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (keyValuePair.Length != 2)
                {
                    throw new BadRequestException("Form data error.");
                }

                var formDataKey = keyValuePair[0];
                var formDataValue = keyValuePair[1];

                this.FormData[formDataKey] = formDataValue;
            }
        }

        private void ParseQueryParameters(string url)
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            var queryParams = this.Url
                .Split(new char[] { '?', '#' })
                .Skip(1).Take(1).ToArray()[0];

            if (string.IsNullOrEmpty(queryParams))
            {
                throw new BadRequestException("Query parameters are null or empty.");
            }
            var queryKeyValuePairs = queryParams.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var queryKeyValuePair in queryKeyValuePairs)
            {
                var keyValuePair = queryKeyValuePair.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (keyValuePair.Length != 2)
                {
                    throw new BadRequestException("Query key value error.");
                }
                var queryKey = keyValuePair[0];
                var queryValue = keyValuePair[1];

                this.QueryData[queryKey] = queryValue;
            }
        }

        private void ParseHeaders(string[] requestHeaders)
        {
            if (!requestHeaders.Any())
            {
                throw new BadRequestException("Request contains no headers!");
            }

            foreach (var requestHeader in requestHeaders)
            {
                if (string.IsNullOrEmpty(requestHeader))
                {
                    return;
                }

                var splitRequestHeader = requestHeader.Split(": ", StringSplitOptions.RemoveEmptyEntries);
                var requestHeaderKey = splitRequestHeader[0];
                var requestHeaderValue = splitRequestHeader[1];

                this.Headers.Add(new HttpHeader(requestHeaderKey, requestHeaderValue));
            }
        }

        private void ParseRequestPath()
        {
            var path = this.Url.Split(new char[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            this.Path = path;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            if (string.IsNullOrEmpty(requestLine[1]))
            {
                throw new BadRequestException("Request url is null or empty.");
            }

            this.Url = requestLine[1];
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            var parseResult = Enum.TryParse<HttpRequestMethod>(requestLine[0], out var parsedRequestMethod);

            if (!parseResult)
            {
                throw new BadRequestException("Http Request Method is invalid.");
            }

            this.RequestMethod = parsedRequestMethod;
        }

        private bool ValidateRequestLine(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                throw new BadRequestException("Request line is empty!");
            }

            if (requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneProtocolFragment)
            {
                return true;
            }

            return false;
        }        
    }
}
