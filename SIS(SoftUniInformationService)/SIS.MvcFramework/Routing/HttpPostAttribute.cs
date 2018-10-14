using SIS.Http.Enums;
using System;

namespace SIS.MvcFramework.Routing
{
    public class HttpPostAttribute:HttpAttrubute
    {
        public HttpPostAttribute(string path)
            :base(path)
        {
        }

        public override HttpRequestMethod Method => HttpRequestMethod.POST;
    }
}
