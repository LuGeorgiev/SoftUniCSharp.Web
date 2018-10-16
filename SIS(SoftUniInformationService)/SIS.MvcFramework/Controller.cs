using Services;
using Services.Contracts;
using SIS.Http.Cookies;
using SIS.Http.Enums;
using SIS.Http.Headers;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        private const string RootDirectoryRelativePath = "../../../";
        private const string ViewFolderName = "Views";
        private const string ControlerDefaultName = "Controller";
        private const string LayoutFimeName = "_Layout";
        private const string HtmlFileExtension = ".html";
        private const string DirectorySeparator = "/";
                

        protected Controller()
        {
            this.ViewBag = new Dictionary<string, string>();
            this.Response = new HttpResponse();
        }

        protected IDictionary<string, string> ViewBag { get; set; }

        public IUserCookieService UserCookieService { get; internal set; }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        protected string User
        {
            get
            {
                return GetUsername();
            }
        }

        public bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username");
        }

        protected void SignInUser(string username)
        {
            this.Request.Session.AddParameter("username", username);
            var userCookieValue = this.UserCookieService.GetUserCookieContent(username);
            this.Response.Cookies.Add(new HttpCookie("Irunes_auth", userCookieValue));
        }

        private string GetUsername()
        {
            if (!this.Request.Cookies.ContainsCookie("Irunes_auth"))
            {
                return null;
            }
            var cookie = this.Request.Cookies.GetCookie("Irunes_auth");
            var cookieContent = cookie.Value;
            var userName = this.UserCookieService.GetUserDate(cookieContent);

            return userName;
        }

        private string GetCurrentControllerName() =>
            this.GetType().Name.Replace(ControlerDefaultName, string.Empty);

        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Response.Content = content;
            this.Response.StatusCode = HttpResponseStatusCode.Ok;

            return this.Response;
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader("Location", location));
            this.Response.StatusCode = HttpResponseStatusCode.SeeOther;

            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader("Content-Type", "text/plain"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            this.Response.StatusCode = HttpResponseStatusCode.Ok;

            return this.Response;
        }

        private void PrepareHtmlResult(string content)
        {
            this.Response.StatusCode = HttpResponseStatusCode.Ok;
            this.Response.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
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

            if (!System.IO.File.Exists(filePath))
            {
                return new BadRequestResult($"View {viewName} not found.", HttpResponseStatusCode.NotFound);
            }

            string viewContent = BuildViewContent(filePath);

            var layoutContent = System.IO.File.ReadAllText(layoutViewPath);

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
            this.PrepareHtmlResult(view);

            return this.Response;
        }


        private string BuildViewContent(string filePath)
        {
            var viewContent = System.IO.File.ReadAllText(filePath);


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
