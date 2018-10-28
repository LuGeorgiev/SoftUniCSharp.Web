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
        
        public IHttpResponse Index()
        {
            if (this.User!=null)
            {                               

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
                     Tasks = tasks
                };

                return this.View("/Home/LoggedInIndex", model);
            }

            return this.View();
        }
    }
}
