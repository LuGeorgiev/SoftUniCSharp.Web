using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Api
{
    public interface IHttpHandlingContext
    {       
        IHttpResponse Handle(IHttpRequest request); 
    }
}
