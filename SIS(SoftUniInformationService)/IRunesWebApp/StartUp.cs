
namespace IRunesWebApp
{
    using IRunesWebApp.Controllers;
    using SIS.Http.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;

    class StartUp
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            ConfigureRouting(serverRoutingTable);           
           
            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }

        private static void ConfigureRouting(ServerRoutingTable serverRoutingTable)
        {
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new RedirectResult("/home/index");
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/home/index"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/users/login"] = request => new UsersController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/users/register"] = request => new UsersController().Register(request);

            serverRoutingTable.Routes[HttpRequestMethod.GET]["/albums/all"] = request => new AlbumsController().All(request);

            //POST
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/users/login"] = request => new UsersController().PostLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/users/register"] = request => new UsersController().PostRegister(request);
        }
    }
}
