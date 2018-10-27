using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;
using TORSHIA.Data;

namespace TORSHIA.Controllers
{
    public class BaseController : Controller
    {
        protected TorshiaDbContext db {get;}

        public BaseController()
        {
            this.db = new TorshiaDbContext();
        }
    }
}
