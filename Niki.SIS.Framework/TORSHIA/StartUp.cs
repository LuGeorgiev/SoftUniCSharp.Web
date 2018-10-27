using SIS.MvcFramework;
using SIS.MvcFramework.Logger;
using SIS.MvcFramework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace TORSHIA
{
    public class StartUp : IMvcApplication
    {
        public void Configure()
        {
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<ILogger, ConsoleLogger>();
        }
    }
}
