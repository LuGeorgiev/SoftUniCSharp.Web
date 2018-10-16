using Services;
using Services.Contracts;
using Services.Logger;
using Services.Logger.Contracts;
using SIS.MvcFramework;

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
            collection.AddService<ILogger, FileLogger>();
        }
    }
}
