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
        private IDictionary<string,HttpCookie> cookiesRepository;

        public HttpCookieCollection()
        {
            this.cookiesRepository = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException("Invalid cookie");
            }
            if (!this.ContainsCookie(cookie.Key))
            {
                this.cookiesRepository[cookie.Key]= cookie;
            }
        }

        public bool ContainsCookie(string key)
            => this.cookiesRepository.ContainsKey(key);

        public HttpCookie GetCookie(string key)
        {
            if (!this.ContainsCookie(key))
            {
                return null;
            }

           return this.cookiesRepository[key];
        }


        public bool HasCookies()
            => this.cookiesRepository.Any();

        public override string ToString()
            => string.Join(", ", this.cookiesRepository.Values);

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookiesRepository)
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
