namespace SIS.Http.Headers
{
    using Contracts;
    using System;
    using System.Collections.Generic;

    public class HttpHeadersCollection : IHttpHeadersCollection
    {
        private readonly IDictionary<string, HttpHeader> headers;

        public HttpHeadersCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            if (header != null && !string.IsNullOrEmpty(header.Key) &&
                !string.IsNullOrEmpty(header.Value) && !this.ContainsHeader(header.Key))
            {
                this.headers.Add(header.Key, header);
            }
            else
            {
                throw new ArgumentException($"Header data is null or header is already added.");
            }
        }

        public bool ContainsHeader(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"{nameof(key)} cannot be null.");
            }
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"{nameof(key)} cannot be null.");
            }

            if (this.ContainsHeader(key))
            {
                return this.headers[key];
            }
            return null;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.headers.Values);
        }

    }
}
