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

    public abstract class BaseController
    {
        private const string RootDirectoryRelativePath = "../../../";
        private const string ViewFolderName = "Views";
        private const string ControlerDefaultName = "Controller";
        private const string HtmlFileExtension = ".html";
        private const string DirectorySeparator = "/";

        protected IRunesContext Context { get; set; }
        protected readonly IHashService hashService;
        protected readonly IUserCookieService userCookieService;
        protected IDictionary<string, string> ViewBag { get; set; }

        public BaseController()
        {
            this.Context = new IRunesContext();
            this.hashService = new HashService();
            this.userCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        public bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameters("username");
        }

        public void SignInUser(string username, IHttpRequest request, IHttpResponse response)
        {
            request.Session.AddParameter("username", username);//TODO to check if this is needed

            var userCookieValue = this.userCookieService.GetUserCookieContent(username);
            response.Cookies.Add(new HttpCookie("Irunes_auth", userCookieValue));
        }

        private string GetCurrentControllerName() =>
            this.GetType().Name.Replace(ControlerDefaultName, string.Empty);

        protected IHttpResponse View([CallerMemberName] string viewName="")
        {
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
                return new BadRequestResult($"View {viewName} not found.", HttpStatusCode.NotFound);
            }
            var fileContent=File.ReadAllText(filePath);


            foreach (var key in ViewBag.Keys)
            {
                if (fileContent.Contains($"{{{{{key}}}}}"))
                {
                    fileContent = fileContent.Replace($"{{{{{key}}}}}", this.ViewBag[key]);
                }
            }

            var response = new HtmlResult(fileContent, HttpStatusCode.OK);
            return response;
        }
    }
}
