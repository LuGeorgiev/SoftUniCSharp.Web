using BasicExam04112018.ViewModels.Home;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicExam04112018.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.IsLoggedIn)
            {
                var loggedUsername = this.User.Username;

                var pendingPackages = this.Db.Packages
                    .Where(x=>x.Recepient.Username==loggedUsername
                    &&x.Status==Models.Status.Pending)
                    .Select(
                    x => new ProductViewModel
                    {
                        Description=x.Description,
                        Id=x.Id
                    })
                    .ToList();

                var shippedPackages = this.Db.Packages
                    .Where(x => x.Recepient.Username == loggedUsername
                    && x.Status == Models.Status.Shipped)
                    .Select(
                    x => new ProductViewModel
                    {
                        Description = x.Description,
                        Id = x.Id
                    })
                    .ToList();

                var deliveredPackages = this.Db.Packages
                    .Where(x => x.Recepient.Username == loggedUsername
                    && x.Status == Models.Status.Delivered)
                    .Select(
                    x => new ProductViewModel
                    {
                        Description = x.Description,
                        Id = x.Id
                    })
                    .ToList();


                var model = new IndexViewModel
                {
                    Delivered = deliveredPackages,
                    Pending = pendingPackages,
                    Shipped = shippedPackages
                };

                return this.View("Home/IndexLoggedIn", model);
            }

            return this.View();
        }
    }
}
