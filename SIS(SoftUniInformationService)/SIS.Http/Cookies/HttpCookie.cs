﻿using System;
using System.Text;

namespace SIS.Http.Cookies
{
    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationDays)
        {
            this.Key = key;
            this.Value = value;
            this.IsNew = true;
            this.Expires = DateTime.UtcNow.AddDays(expires);
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays)
            : this(key, value, expires)
        {
            this.IsNew = isNew;
        }

        public string Key { get; }

        public string Value { get; }

        public DateTime Expires { get; private set; }

        public bool IsNew { get; }

        public override string ToString() => $"{this.Key}={this.Value}; Expires={this.Expires.ToLongTimeString()}";

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1);
        }
    }
}
