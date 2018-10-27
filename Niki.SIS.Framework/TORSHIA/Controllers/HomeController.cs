using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TORSHIA.ViewModels.Home;
using TORSHIA.ViewModels.Tasks;

namespace TORSHIA.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("/Home/Index")]
        public IHttpResponse Index()
        {
            if (this.User!=null)
            {
                var user = this.db.Users
                    .FirstOrDefault(x => x.Username == this.User.Username);

                string role = user.Role.ToString();

                var tasks = this.db.Tasks
                    .Where(x => x.IsReported == false)
                    .Select(x=>new TaskListingViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Level = x.AffectedSectors.Count()
                    })
                    .ToArray();

                LoggedInViewModel model = new LoggedInViewModel
                {
                     UserRole=role,
                     Tasks = tasks
                };

                return this.View("/Home/LoggedInIndex", model);
            }

            return this.View("/Home/Index");
        }

        [HttpGet("/")]
        public IHttpResponse RootIndex()
        {
            return this.Index();
        }
    }
}
