using SIS.MvcFramework;
using System;

namespace Chushka
{
    class Program
    {
        static void Main(string[] args)
        {            
            WebHost.Start(new StartUp());            
        }
    }
}
