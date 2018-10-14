namespace IRunesWebApp.Controllers
{
    using SIS.Http.Requests.Contracts;
    using SIS.Http.Responses.Contracts;
    using Extensions;
    using SIS.WebServer.Results;
    using System.Net;
    using System.Collections.Generic;
    using SIS.MvcFramework.Routing;

    public class HomeController : BaseController
    {
        [HttpGet("/Home/Index")]
        public IHttpResponse Index()
        {
            if (this.IsAuthenticated(this.Request))
            {
                var username = this.Request.Session.GetParameter("username");
                this.ViewBag["username"] = username.ToString();

                return this.View("IndexLoggedIn");
            }

            return this.View();
        }
    }
}
