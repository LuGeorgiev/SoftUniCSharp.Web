namespace SIS.Http.Cookies
{
    using Contracts;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private IDictionary<string,HttpCookie> CookieRepository;

        public HttpCookieCollection()
        {
            this.CookieRepository = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
            =>this.CookieRepository.Add(cookie.Key, cookie);


        public bool ContainsCookie(string key)
            => this.CookieRepository.ContainsKey(key);

        public HttpCookie GetCookie(string key)
            => this.CookieRepository[key];

        public bool HasCookies()
            => this.CookieRepository.Any();

        public override string ToString()
            => string.Join(", ", this.CookieRepository.Values);
    }
}
