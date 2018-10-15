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
    using SIS.Framework.Controllers;

    public abstract class BaseController:Controller
    {
        private const string RootDirectoryRelativePath = "../../../";
        private const string ViewFolderName = "Views";
        private const string ControlerDefaultName = "Controller";
        private const string LayoutFimeName = "_Layout";
        private const string HtmlFileExtension = ".html";
        private const string DirectorySeparator = "/";

        private readonly IUserCookieService userCookieService;
        

        public BaseController()
        {
            this.Context = new IRunesContext();
            this.userCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected IDictionary<string, string> ViewBag { get; set; }

        protected IRunesContext Context { get; set; }

        public bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        public void SignInUser(string username, IHttpResponse response, IHttpRequest request)
        {
            request.Session.AddParameter("username", username);
            var userCookieValue = this.userCookieService.GetUserCookieContent(username);
            response.Cookies.Add(new HttpCookie("Irunes_auth", userCookieValue));
        }

        private string GetCurrentControllerName() =>
            this.GetType().Name.Replace(ControlerDefaultName, string.Empty);

        protected IHttpResponse View([CallerMemberName] string viewName="")
        {            
            var layoutViewPath = RootDirectoryRelativePath +
                                ViewFolderName +
                                DirectorySeparator +
                                LayoutFimeName +
                                HtmlFileExtension;

            //../../../Views/ControllerName/ActionName.html
            string filePath = RootDirectoryRelativePath +
                                ViewFolderName +
                                DirectorySeparator +
                                this.GetCurrentControllerName() +
                                DirectorySeparator +
                                viewName +
                                HtmlFileExtension;

            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"View {viewName} not found.", HttpResponseStatusCode.NotFound);
            }

            string viewContent = BuildViewContent(filePath);

            var layoutContent = File.ReadAllText(layoutViewPath);

            //TODO navigation to be re-worked
            //var layoutNavigation = "";
            //if (this.ViewBag.ContainsKey("username"))
            //{
            //    layoutNavigation = File.ReadAllText("../../../Views/Navigation/Logged.html");
            //}
            //else
            //{
            //    layoutNavigation = File.ReadAllText("../../../Views/Navigation/NotLogged.html");

            //}
            var view = layoutContent.Replace("@RenderBody()", viewContent);
            //view = view.Replace("@RenderNavigation()", layoutNavigation);            

            var response = new HtmlResult(view, HttpResponseStatusCode.Ok);
            return response;
        }
       

        private string BuildViewContent(string filePath)
        {
            var viewContent = File.ReadAllText(filePath);


            foreach (var key in ViewBag.Keys)
            {
                var dynamicPlaceholder = $"{{{{{key}}}}}";

                if (viewContent.Contains(dynamicPlaceholder))
                {
                    viewContent = viewContent.Replace(dynamicPlaceholder, this.ViewBag[key]);
                }
            }

            return viewContent;
        }
    }
}
