using System;
using SIS.Http.Enums;

namespace SIS.MvcFramework.Routing
{
    public class HttpGetAttribute: HttpAttrubute
    {

        public HttpGetAttribute(string path)
            : base(path)
        {
        }

        public override HttpRequestMethod Method => HttpRequestMethod.GET;
    }
}
