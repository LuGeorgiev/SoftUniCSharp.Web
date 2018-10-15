using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Utilities
{
    public static class ControllerUtilities
    {
        public static string GetControlerName(object controller)
            => controller.GetType()
            .Name
            .Replace(MvcContext.Get.ControllersSuffix, string.Empty);

        //TODO path refactor
        public static string GetViewFullQualifiedName(string controller, string viewName)
            => string.Format("../../../{0}/{1}/{2}.html",
                MvcContext.Get.ViewFolder,
                controller,
                viewName);
    }
}
