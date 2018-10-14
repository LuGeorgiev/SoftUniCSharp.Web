using IRunesWebApp.Controllers;
using SIS.Http.Enums;
using SIS.MvcFramework;
using SIS.WebServer.Routing;

namespace IRunesWebApp
{
    public class StartUp : IMvcApplication
    {
        public void Configure()
        {
        //    //GET    
        //    rounting.Routes[HttpRequestMethod.GET]["/Home/Index"] = request => new HomeController() { Request = request }.Index();
        //    rounting.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController() { Request = request }.Index();

        //    rounting.Routes[HttpRequestMethod.GET]["/Users/Login"] = request => new UsersController() { Request = request }.Login();
        //    rounting.Routes[HttpRequestMethod.GET]["/Users/Register"] = request => new UsersController() { Request = request }.Register();
        //    rounting.Routes[HttpRequestMethod.GET]["/Users/Logout"] = request => new UsersController() { Request = request }.Logout();

        //    rounting.Routes[HttpRequestMethod.GET]["/Albums/All"] = request => new AlbumsController() { Request = request }.All();
        //    rounting.Routes[HttpRequestMethod.GET]["/Albums/Create"] = request => new AlbumsController() { Request = request }.Create();
        //    rounting.Routes[HttpRequestMethod.GET]["/Albums/Details"] = request => new AlbumsController() { Request = request }.Details();

        //    rounting.Routes[HttpRequestMethod.GET]["/Tracks/Create"] = request => new TracksController() { Request = request }.Create();
        //    rounting.Routes[HttpRequestMethod.GET]["/Tracks/Details"] = request => new TracksController() { Request = request }.Details();


        //    //POST
        //    rounting.Routes[HttpRequestMethod.POST]["/Users/Login"] = request => new UsersController() { Request = request }.LoginPost();
        //    rounting.Routes[HttpRequestMethod.POST]["/Users/Register"] = request => new UsersController() { Request = request }.RegisterPost();
        //    rounting.Routes[HttpRequestMethod.POST]["/Albums/Create"] = request => new AlbumsController() { Request = request }.CreatePost();
        //    rounting.Routes[HttpRequestMethod.POST]["/Tracks/Create"] = request => new TracksController() { Request = request }.CreatePost();
        }

        public void ConfigureServices()
        {
            throw new System.NotImplementedException();
        }
    }
}
