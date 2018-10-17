
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
            //ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            //MvcContext.Get.AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            //ConfigureRouting(serverRoutingTable);           
           
            IHttpHandler handler = new ControllerRouter();
            Server server = new Server(8000, handler);
            MvcEngine.Run(server);

            //server.Run();
        }

        private static void ConfigureRouting(ServerRoutingTable serverRoutingTable)
        {
            //GET        
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Home/Index"] = request => new RedirectResult("/");
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController().Index(request);

            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Users/Login"] = request => new UsersController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Users/Register"] = request => new UsersController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Users/Logout"] = request => new UsersController().Logout(request);

            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Albums/All"] = request => new AlbumsController().All(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Albums/Create"] = request => new AlbumsController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Albums/Details"] = request => new AlbumsController().Details(request);

            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Tracks/Create"] = request => new TracksController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Tracks/Details"] = request => new TracksController().Details(request);


            //POST
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/Users/Login"] = request => new UsersController().LoginPost(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/Users/Register"] = request => new UsersController().RegisterPost(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/Albums/Create"] = request => new AlbumsController().CreatePost(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/Tracks/Create"] = request => new TracksController().CreatePost(request);
        }
    }
}
