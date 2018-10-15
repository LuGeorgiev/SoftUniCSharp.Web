using SIS.Framework;
using SIS.Framework.Routes;
using SIS.WebServer;
using System;

namespace SIS.Demo
{
    class Launcer
    {
        static void Main(string[] args)
        {

            var server = new Server(8000, new ControllerRouter());

            MvcEngine.Run(server);
        }
    }
}
