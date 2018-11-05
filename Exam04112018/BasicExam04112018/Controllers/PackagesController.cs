using BasicExam04112018.Models;
using BasicExam04112018.ViewModels.Packages;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicExam04112018.Controllers
{
    public class PackagesController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            var users = this.Db.Users.Select(x => new UserDropDownListingModel
            {                 
                Username = x.Username
            })
            .ToList();


            var model = new UserToChooseModel()
            {
                Users=users    
            };
            
            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(CreatePackagePostModel model)
        {
            var recepient = this.Db.Users.FirstOrDefault(x => x.Username.ToLower() == model.Username.ToLower());

            if (recepient==null)
            {
                return this.BadRequestError("User do not exist");
            }

            if (!float.TryParse(model.Weight,out var weight))
            {
                return this.BadRequestError("Weight i snot correct");
            }

            var package = new Package()
            {
                 Description=model.Description,
                 RecepientId=recepient.Id,
                 ShippingAddress=model.ShippingAddress,
                 Status=Status.Pending,
                 EstemetedDeliveryDate=null,
                 Weight=weight
            };

            this.Db.Packages.Add(package);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var package = this.Db.Packages
                .FirstOrDefault(x => x.Id == id);
            if (package==null)
            {
                return this.BadRequestError($"Package with Id : {id} was not found");
            }

            var estimateDelivery = "";
            if (package.EstemetedDeliveryDate == null)
            {
                estimateDelivery = "N/A";
            }
            else if (package.Status==Status.Delivered|| package.Status == Status.Acquired)
            {
                estimateDelivery = "Delivered";
            }
            else
            {
                estimateDelivery = package.EstemetedDeliveryDate.Value.ToString("dd/MM/yyyy");
            }
            var model = new PackageDetailsViewModel()
            {
                ShippingAddress = package.ShippingAddress,
                Status = package.Status.ToString(),
                EstemetedDeliveryDate=estimateDelivery,
                Weight =package.Weight.ToString("F2"),
                Recepient = package.Recepient.Username,
                Description=package.Description
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Pending()
        {
            var pendingPackages = this.Db.Packages
                .Where(x => x.Status == Status.Pending)
                .Select(x=> new PackageViewModelByStatus
                {
                    Id=x.Id,
                    Description=x.Description,
                    Weight=x.Weight.ToString("F2"),
                    ShippingAddress = x.ShippingAddress,
                    Username =x.Recepient.Username
                })
                .ToList();

            var model = new ListingViewModelByStatus()
            {
                Packages=pendingPackages
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Ship(int id)
        {
            var package = this.Db.Packages
                .FirstOrDefault(x => x.Id == id);

            if (package==null)
            {
                return this.BadRequestError("Package was not found");
            }

            package.Status = Status.Shipped;
            this.Db.SaveChanges();

            return Redirect("/packages/pending");
        }

        [Authorize("Admin")]
        public IHttpResponse Shipped()
        {
            var shippedPackages = this.Db.Packages
               .Where(x => x.Status == Status.Shipped)
               .Select(x => new PackageViewModelByStatus
               {
                   Id = x.Id,
                   Description = x.Description,
                   Weight = x.Weight.ToString("F2"),
                   ShippingAddress = x.ShippingAddress,
                   Username = x.Recepient.Username
               })
               .ToList();

            var model = new ListingViewModelByStatus()
            {
                Packages = shippedPackages
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Deliver(int id)
        {
            var package = this.Db.Packages
                .FirstOrDefault(x => x.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Package was not found");
            }

            package.Status = Status.Delivered;
            this.Db.SaveChanges();

            return Redirect("/packages/shipped");
        }

        [Authorize("Admin")]
        public IHttpResponse Delivered()
        {
            var deliveredPackages = this.Db.Packages
              .Where(x => x.Status == Status.Delivered)
              .Select(x => new PackageViewModelByStatus
              {
                  Id = x.Id,
                  Description = x.Description,
                  Weight = x.Weight.ToString("F2"),
                  ShippingAddress = x.ShippingAddress,
                  Username = x.Recepient.Username
              })
              .ToList();

            var model = new ListingViewModelByStatus()
            {
                Packages = deliveredPackages
            };

            return this.View(model);
        }
    }
}
