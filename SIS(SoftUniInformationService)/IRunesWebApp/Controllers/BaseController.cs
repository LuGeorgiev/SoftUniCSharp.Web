namespace IRunesWebApp.Controllers
{
    using IRunesWebApp.Data;
    using Services;
    using Services.Contracts;
    using SIS.Http.Cookies;
    using SIS.Http.Requests.Contracts;
    using SIS.Http.Responses.Contracts;
    using SIS.WebServer.Results;
    using System.IO;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System;
    using IRunesWebApp.Extensions;
    using SIS.Http.Enums;
    using SIS.MvcFramework;

    public abstract class BaseController : Controller
    {                
        protected IRunesContext Context { get; set; }

        public BaseController()
            :base()
        {
            this.Context = new IRunesContext();
        }
    }
}
