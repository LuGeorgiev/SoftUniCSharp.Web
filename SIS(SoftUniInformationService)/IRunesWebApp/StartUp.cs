
namespace IRunesWebApp
{
    using IRunesWebApp.Controllers;
    using SIS.Framework;
    using SIS.Framework.Routes;
    using SIS.Http.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Api;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;
    using System.Reflection;

    class StartUp
    {
        static void Main(string[] args)
        {
            var handlingContext = new HttpRouteHandlingContext(
                 new ControllerRouter(),
                 new ResourceRouter());
            Server server = new Server(80, handlingContext);
            var engine = new MvcEngine();
            engine.Run(server);
        }
    }
}
