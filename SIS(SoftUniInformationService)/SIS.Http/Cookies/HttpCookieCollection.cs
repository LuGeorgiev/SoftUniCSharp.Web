namespace SIS.Http.Cookies
{
    using Contracts;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private const string HttpCookieStringSeparator = "; ";

        private IDictionary<string,HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException("Invalid cookie");
            }
          
                this.cookies.Add(cookie.Key, cookie);
            
        }

        public bool ContainsCookie(string key)
            => this.cookies.ContainsKey(key);

        public HttpCookie GetCookie(string key)
        {
            if (!this.ContainsCookie(key))
            {
                return null;
            }

           return this.cookies[key];
        }


        public bool HasCookies()
            => this.cookies.Any();

        public override string ToString()
            => string.Join(HttpCookieStringSeparator, this.cookies.Values);

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookies)
            {
                yield return cookie.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
