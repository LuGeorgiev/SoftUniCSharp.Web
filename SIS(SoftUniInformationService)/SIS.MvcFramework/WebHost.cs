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
    using Services;
    using Services.Contracts;
    using System.Collections.Generic;
    using SIS.MvcFramework.Extensions;
    using System.Threading;
    using System.Globalization;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var dependencyContainer = new ServiceCollection();
            application.ConfigureServices(dependencyContainer);

            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application, dependencyContainer);
            application.Configure();

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable routingTable,IMvcApplication application, IServiceCollection serviceCollection)
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
                        (request)=>ExecuteAction(controller,methodInfo, request, serviceCollection));

                    Console.WriteLine($"Route registered: {controller.FullName}.{httpAttribute.Method}=> Action: {httpAttribute.Path}");

                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            //1. Controller instance
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;
            if (controllerInstance == null)
            {
                Console.WriteLine("Controller not found");
            }

            //2. Set Request
            controllerInstance.Request = request;
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();

            //NB extarct and populate parameters
            List<object> actionParameterObjects = ExtractParameters(methodInfo, request, serviceCollection);

            //3. Invoke actionName
            var httpResponse = methodInfo.Invoke(controllerInstance, actionParameterObjects.ToArray()) as IHttpResponse;

            //4. Return action result
            return httpResponse;

        }

        private static List<object> ExtractParameters(MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            var actionParameters = methodInfo.GetParameters();
            var actionParameterObjects = new List<Object>();
            foreach (var actionParameter in actionParameters)
            {
                if (actionParameter.ParameterType.IsValueType 
                    || Type.GetTypeCode(actionParameter.ParameterType)==TypeCode.String)
                {
                    string stringValue = GetRequestData(request, actionParameter.Name);
                    actionParameterObjects.Add(TryParse(stringValue,actionParameter.ParameterType));
                }
                else
                {
                    var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);
                    var properties = actionParameter.ParameterType.GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        string stringValue = GetRequestData(request, propertyInfo.Name);
                        //Check type of properties
                        //Decimal
                        //int, char, double, DateTime
                        var typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
                        var value = TryParse(stringValue, propertyInfo.PropertyType);

                        propertyInfo.SetMethod.Invoke(instance, new object[] { value });
                    }

                    //Populate properties from ViewModels
                    actionParameterObjects.Add(instance);
                }

            }

            return actionParameterObjects;
        }

        private static string GetRequestData(IHttpRequest request, string key)
        {
            key = key.ToLower();
            string stringValue = null;
            if (request.FormData.Any(x => x.Key.ToLower() == key))
            {
                stringValue = request.FormData
                    .First(x => x.Key.ToLower() == key)
                    .Value
                    .ToString()
                    .UrlDecode();
            }
            else if (request.QueryData.Any(x => x.Key.ToLower() == key))
            {
                stringValue = request.QueryData
                    .First(x => x.Key.ToLower() == key)
                    .Value
                    .ToString()
                    .UrlDecode();
            }

            return stringValue;
        }

        private static object TryParse(string stringValue, Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            object value = null;
            switch (typeCode)
            {
                case TypeCode.Char:
                    if (char.TryParse(stringValue, out var charValue))
                        value = charValue;
                    break;
                case TypeCode.DateTime:
                    if (DateTime.TryParse(stringValue, out var dateValue))
                        value = dateValue;
                    break;
                case TypeCode.Decimal:
                    if (decimal.TryParse(stringValue, out var decimalValue))
                        value = decimalValue;
                    break;
                case TypeCode.Double:
                    if (double.TryParse(stringValue, out var doubleValue))
                        value = doubleValue;
                    break;
                case TypeCode.Int32:
                    if (int.TryParse(stringValue, out var intValue))
                        value = intValue;
                    break;
                case TypeCode.Int64:
                    if (long.TryParse(stringValue, out var longValue))
                        value = longValue;
                    break;
                case TypeCode.String:
                    value = stringValue;
                    break;
                default:
                    break;
            }

            return value;
        }
    }
}
