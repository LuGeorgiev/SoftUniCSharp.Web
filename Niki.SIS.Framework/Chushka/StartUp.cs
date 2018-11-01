using Castle.Core.Logging;
using SIS.MvcFramework;
using SIS.MvcFramework.Services;

namespace Chushka
{
    public class StartUp: IMvcApplication
    {
        public MvcFrameworkSettings Configure()
        {
            return new MvcFrameworkSettings() { PortNumber=8000};
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<ILogger, ConsoleLogger>();
            collection.AddService<IHashService, HashService>();
            collection.AddService<IUserCookieService, UserCookieService>();
        }
    }
}
