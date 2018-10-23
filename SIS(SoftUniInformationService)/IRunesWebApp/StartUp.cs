
namespace IRunesWebApp
{
    using global::Services;
    using global::Services.Contracts;
    using IRunesWebApp.Controllers;
    using IRunesWebApp.Services;
    using IRunesWebApp.Services.Contracts;
    using SIS.Framework;
    using SIS.Framework.Routes;
    using SIS.Http.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Api;
    using SIS.WebServer.Results;
    using SIS.WebServer.Routing;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    class StartUp
    {
        static void Main(string[] args)
        {
            var dependencyMap = new Dictionary<Type, Type>();
            IDependencyContainer dependencyContainer = new DependencyContainer(dependencyMap);
            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUsersService, UsersService>();

            var handlingContext = new HttpRouteHandlingContext(
                new ControllerRouter(dependencyContainer),
                new ResourceRouter());
            Server server = new Server(80, handlingContext);
            var engine = new MvcEngine();
            engine.Run(server);
        }
    }
}
