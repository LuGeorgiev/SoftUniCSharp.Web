using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework.Routing;

namespace SIS.MvcFramework
{
    using WebServer;
    using WebServer.Routing;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            //TODO after services configured to be uncommented
            //application.ConfigureServices();

            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application);
            application.Configure();

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable routingTable,IMvcApplication application)
        {
           var controllers = application.GetType().Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Controller)));

            foreach (var controller in controllers)
            {
                var getMethods = controller
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.CustomAttributes
                        .Any(ca=>ca.AttributeType.IsSubclassOf(typeof(HttpAttrubute))));

                foreach (var methodInfo in getMethods)
                {
                    var httpAttribute = (HttpAttrubute)methodInfo.GetCustomAttributes(true)
                        .FirstOrDefault(ca => ca.GetType().IsSubclassOf(typeof(HttpAttrubute)));

                    routingTable.Add(httpAttribute.Method,httpAttribute.Path, 
                        (request)=>ExecuteAction(controller,methodInfo, request));

                    Console.WriteLine($"Route registered: {controller.FullName}.{httpAttribute.Method}=> Action: {httpAttribute.Path}");

                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request)
        {
            //1. Controller instance
            var controllerInstance = Activator.CreateInstance(controllerType) as Controller;
            if (controllerInstance ==null)
            {
                Console.WriteLine("Controller not found");
            }

            //2. Set Request
            var controlerAction = controllerInstance.Request = request;

            //3. Invoke actionName
            var httpResponse = methodInfo.Invoke(controllerInstance, new object[] { }) as IHttpResponse;

            //4. Return action result
            return httpResponse;
            
        }
    }
}
