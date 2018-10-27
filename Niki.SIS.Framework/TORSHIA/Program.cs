using SIS.MvcFramework;
using System;

namespace TORSHIA
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }
    }
}
