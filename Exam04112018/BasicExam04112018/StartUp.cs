using SIS.MvcFramework;
using SIS.MvcFramework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicExam04112018
{
    public class StartUp : IMvcApplication
    {
        public MvcFrameworkSettings Configure()
        {
            return new MvcFrameworkSettings() { PortNumber=8000};
        }

        public void ConfigureServices(IServiceCollection collection)
        {
        }
    }
}
