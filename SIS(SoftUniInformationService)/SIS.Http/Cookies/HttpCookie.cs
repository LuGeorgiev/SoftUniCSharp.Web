﻿using System;
using System.Text;

namespace SIS.Http.Cookies
{
    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;
        private const string HttpCookieDefaultPath = "/";

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationDays, string path=HttpCookieDefaultPath)
        {
            this.Key = key;
            this.Value = value;
            this.Expires = DateTime.UtcNow.AddDays(expires);
            this.IsNew = true;
            this.Path = path;
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays, string path = HttpCookieDefaultPath )
            :this(key, value,expires)
        {
            this.IsNew = isNew;
        }

        public string Path { get; set; }

        public string Key { get;  }

        public string Value { get;  }

        public DateTime Expires { get; private set; }

        public bool IsNew { get;  }

        public bool HttpOnly { get; set; } = true;

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{this.Key}={this.Value}; Expires={this.Expires:R}");

            if (this.HttpOnly)
            {
                sb.Append("; HttpOnly");
            }

            sb.Append($"; Path={this.Path}");

            return sb.ToString();
        }

    }
}
