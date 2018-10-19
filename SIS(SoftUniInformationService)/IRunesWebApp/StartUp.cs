using Services;
using Services.Contracts;
using Services.Logger;
using Services.Logger.Contracts;
using SIS.MvcFramework;
using System;

namespace IRunesWebApp
{
    public class StartUp : IMvcApplication
    {
        public void Configure()
        {            
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<IHashService, HashService>();
            collection.AddService<IUserCookieService, UserCookieService>();
            //collection.AddService<ILogger, FileLogger>();
            collection.AddService<ILogger>(() => new FileLogger($"Log_{DateTime.Now.Date}.txt"));
        }
    }
}
