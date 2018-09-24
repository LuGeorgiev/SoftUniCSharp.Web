using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RequestParser
{
    class StartParsing
    {
        static void Main(string[] args)
        {
            var routes = new Dictionary<string, HashSet<string>>();
            var input = "";

            while ((input=Console.ReadLine()).ToLower()!="end")
            {
                var routeEntry = input.ToLower().Split("/", StringSplitOptions.RemoveEmptyEntries);

                var controller = routeEntry[0];
                var httpMethod = routeEntry[1];

                if (!routes.ContainsKey(httpMethod))
                {
                    routes.Add(httpMethod, new HashSet<string>());
                }
                routes[httpMethod].Add(controller);
            }

            var requestString = Console.ReadLine().ToLower();
            var splittedRequest = requestString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var requestHttpMetod = splittedRequest[0];
            var requestHttpController = splittedRequest[1].Trim('/');
            var requestHttpProtocol = splittedRequest[2];

            if (routes.ContainsKey(requestHttpMetod))
            {
                var endPoint = routes[requestHttpMetod]
                    .FirstOrDefault(x => x == requestHttpController);
                if (endPoint!=null)
                {
                    var succeesHttpResponse = $"{requestHttpProtocol} {(int)HttpStatusCode.OK} {HttpStatusCode.OK} {Environment.NewLine}"+
                        $"Content - Length: {HttpStatusCode.OK.ToString().Length} {Environment.NewLine}"+
                        $"Content-Type: text/plain {Environment.NewLine} {Environment.NewLine}"+
                        $"{HttpStatusCode.OK}";

                    Console.WriteLine(succeesHttpResponse);
                    return;
                }
            }
           
            var errorHttpResponse = $"{requestHttpProtocol} {(int)HttpStatusCode.NotFound} {HttpStatusCode.NotFound} {Environment.NewLine}" +
                    $"Content - Length: {HttpStatusCode.NotFound.ToString().Length} {Environment.NewLine}" +
                    $"Content-Type: text/plain {Environment.NewLine} {Environment.NewLine}" +
                    $"{HttpStatusCode.NotFound}";

            Console.WriteLine(errorHttpResponse);
            
        }
    }
}
