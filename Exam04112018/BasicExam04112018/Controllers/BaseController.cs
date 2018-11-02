using BasicExam04112018.Data;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicExam04112018.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Db = new ExamDbContext();
        }

        public ExamDbContext Db { get; }
    }
}
