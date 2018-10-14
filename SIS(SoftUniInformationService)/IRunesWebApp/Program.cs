
namespace IRunesWebApp
{
    using SIS.MvcFramework;

    class Program
    {
        static void Main(string[] args)
        {
            var mvcApplication = new StartUp();
            WebHost.Start(mvcApplication);
            
        }       
    }
}
