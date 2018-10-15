namespace SIS.Framework.Controllers
{
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Utilities;
    using SIS.Framework.Views;
    using SIS.Http.Requests.Contracts;
    using System.Runtime.CompilerServices;

    public abstract class Controller  
    {
        protected Controller()
        {
        }

        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            var controllerName = ControllerUtilities
                .GetControlerName(this);
            var fullQualifiedName = ControllerUtilities
                .GetViewFullQualifiedName(controllerName, viewName);
            var view = new View(fullQualifiedName);

            return new ViewResult(view);
        }

        protected IRedirectable RedurectToAction(string redirectUrl)
            => new RedirectResult(redirectUrl);
    }
}
