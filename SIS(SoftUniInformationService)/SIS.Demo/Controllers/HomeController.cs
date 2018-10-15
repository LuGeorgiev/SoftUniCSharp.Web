using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo.Controllers
{
    public class HomeController:Controller
    {
        [HttpGetAttribute]
        public IActionResult Index()
        {
            return View();
        }
    }
}
