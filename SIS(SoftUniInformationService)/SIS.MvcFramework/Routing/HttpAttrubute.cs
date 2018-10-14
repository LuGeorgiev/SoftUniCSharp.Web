using SIS.Http.Enums;
using System;

namespace SIS.MvcFramework.Routing
{
    public abstract class HttpAttrubute:Attribute
    {
        protected HttpAttrubute(string path)
        {
            this.Path = path;
        }
        public string Path { get; protected set; }

        public abstract HttpRequestMethod Method { get; }
    }
}
