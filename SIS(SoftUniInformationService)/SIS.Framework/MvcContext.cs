using System;
using System.Reflection;

namespace SIS.Framework
{
    public class MvcContext
    {
        public static MvcContext Instance;

        private MvcContext()
        {
        }

        public static MvcContext Get => Instance == null ? (Instance = new MvcContext()) : Instance;

        public string AssemblyName { get; set; }

        public string ControllersFolder { get; set; } = "Controllers";

        public string ControllersSuffix { get; set; } = "Controller";

        public string ViewFolder { get; set; } = "Views";

        public string ModelsFolder { get; set; } = "Models";

    }
}
