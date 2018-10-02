namespace SIS.Http.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Enums;
    using Headers;
    using Headers.Contracts;
    using Common;
    using Cookies.Contracts;
    using Exceptions;
    using SIS.Http.Cookies;
    using SIS.Http.Session.Contracts;
    using SIS.Http.Session;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            
            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        //TODO Session in request is not described
        public IHttpSession Session { get; set; }

        public Dictionary<string, object> FormData { get; private set; }

        public Dictionary<string, object> QueryData { get; private set; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpRequestMethod RequestMethod { get; private set; }
                

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            string[] requestLine = splitRequestContent[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();

            
            bool requestHasBody = splitRequestContent.Length > 1;
            this.ParseRequestParametters(splitRequestContent[splitRequestContent.Length - 1], requestHasBody);

        }

        private void ParseCookies()
        {
            var headers = this.Headers
                .ToString()
                .Split(Environment.NewLine)
                .ToArray();

            foreach (var headar in headers)
            {
                if (headar.Contains("Cookie"))
                {
                    var cookieIndex = headar.IndexOf("Cookie: ");
                    var cookieString = headar.Substring(cookieIndex + 8);
                    var cookies = cookieString.Split("; ", StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cookie in cookies)
                    {
                        var cookieKey = cookie.Split("=").FirstOrDefault();
                        var cookieValue = cookie.Split("=").LastOrDefault();

                        if (cookieKey==null||cookieValue==null||cookieKey==cookieValue)
                        {
                            throw new BadRequestException();
                        }

                        if (!this.Cookies.ContainsCookie(cookieKey))
                        {
                            this.Cookies.Add(new HttpCookie(cookieKey, cookieValue));
                        }
                    }
                }
            }
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (!requestLine.Any())
            {
                throw new BadRequestException();
            }

            if (requestLine.Length == 3
                && requestLine[2] ==GlobalConstants.HttpOnProtocolFrame)
            {
                return true;
            }
            return false;
        }

        private void ParseRequestPath()
        {
            var path = this.Url.Split('?').FirstOrDefault();
            if (string.IsNullOrEmpty( path))
            {
                throw new BadRequestException();
            }

            this.Path = path;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            if (string.IsNullOrEmpty(requestLine[1])) 
            {
                throw new BadRequestException();
            }

            this.Url = requestLine[1];
        }

        private void ParseRequestMethod(string[] requestLine)
        {         
            var requestMethod = requestLine[0];
            var parseResult =Enum.TryParse<HttpRequestMethod>(requestMethod, out var parseMethod);

            if (!parseResult)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = parseMethod;
        }

        private void ParseHeaders(string[] requestHeaders)
        {
            if (!requestHeaders.Any())
            {
                throw new BadRequestException();
            }
            foreach (var requestHeader in requestHeaders)
            {
                if (string.IsNullOrEmpty(requestHeader))
                {
                    return;
                }

                var splitRequestHeader = requestHeader.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                var key = splitRequestHeader[0];
                var value = splitRequestHeader[1];

                if (string.IsNullOrEmpty(key)||string.IsNullOrEmpty(value))
                {
                    return;
                }

                this.Headers.Add(new HttpHeader(key, value));
            }

        }
        
        private void ParseRequestParametters(string bodyParameters, bool hasBody)
        {
            //TODO following two if statements have to be reviewd
            if (this.Url.Contains("?"))
            {
                this.ParseQueryParameters(this.Url);

                if (hasBody)
                {
                    this.ParseFormDataParameters(bodyParameters);
                }
            }

        }

        private void ParseFormDataParameters(string bodyParameters)
        {           

            var formDataKVP = bodyParameters
                .Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var kvp in formDataKVP)
            {
                var kvpBody = kvp.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (kvpBody.Length != 2)
                {
                    throw new BadRequestException();
                }

                var formDataKey = kvpBody[0];
                var formDataValue = kvpBody[1];

                // should we override or Add
                this.FormData[formDataKey] = formDataValue;
            }
        }

        private void ParseQueryParameters(string url)
        {
            var queryParams = this.Url?
                .Split(new[] { '?' ,'#'})
                .Skip(1)
                .ToArray()[0];
            if (string.IsNullOrEmpty(queryParams))
            {
                throw new BadRequestException();
            }

            var queryKVPs = queryParams
                .Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var kvp in queryKVPs)
            {
                var kvpQuery = kvp.Split('=', StringSplitOptions.RemoveEmptyEntries);
                if (kvpQuery.Length!=2)
                {
                    throw new BadRequestException();
                }

                var queryKey = kvpQuery[0];
                var queryValue = kvpQuery[1];

                // should we override or Add
                this.QueryData[queryKey]= queryValue;
            }
        }
    }
}
