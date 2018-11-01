using Chushka.Data;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chushka.Controllers
{
    public class BaseController: Controller
    {
        protected ChushkaDbContext db { get; }

        public BaseController()
        {
            this.db = new ChushkaDbContext();
        }
    }
}
