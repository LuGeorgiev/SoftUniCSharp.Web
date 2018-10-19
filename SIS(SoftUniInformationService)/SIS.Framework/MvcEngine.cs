using SIS.WebServer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SIS.Framework
{
    public class MvcEngine
    {
        public void Run(Server server)
        {
            MvcContext.Get.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            try
            {
                server.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
