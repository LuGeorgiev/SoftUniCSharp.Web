namespace IRunesWebApp.Controllers
{
    using SIS.Http.Requests.Contracts;
    using SIS.Http.Responses.Contracts;
    using Extensions;
    using SIS.WebServer.Results;
    using System.Net;
    using System.Collections.Generic;

    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                var username = request.Session.GetParameter("username");
                this.ViewBag["username"] = username.ToString();

                return this.View("IndexLoggedIn");
            }

            return this.View();
        }


    }
}
