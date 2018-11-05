using BasicExam04112018.Models;
using BasicExam04112018.ViewModels.Receipts;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicExam04112018.Controllers
{
    public class ReceiptsController : BaseController
    {
        [Authorize]
        public IHttpResponse Aquired(int id)
        {
            var package = this.Db.Packages
                .FirstOrDefault(x => x.Id == id);
            if (package==null)
            {
                return this.BadRequestError("Package was not found");
            }

            var receit = new Receit()
            {
              Fee = (decimal)package.Weight*2.67m,    
              IssuedOn = DateTime.UtcNow,
              PackageId=package.Id,
              RecipientId = package.RecepientId
            };

            package.Status = Status.Acquired;
            this.Db.Receits.Add(receit);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse All()
        {
            var receipts = this.Db.Receits
                .Where(x => x.Recipient.Username == this.User.Username)
                .Select(x=> new ReceitViewModel
                {
                    Id=x.Id,
                    Fee = x.Fee.ToString("F2"),
                    IssuedOn = x.IssuedOn.ToString("dd/MM/yyyy"),
                    Recepient = x.Recipient.Username
                })
                .ToList();
            var model = new ReceiptListingViewModel()
            {
                Receipts=receipts
            };

            return this.View(model);
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var receipt = this.Db.Receits
                .FirstOrDefault(x => x.Id == id
                &&x.Recipient.Username==this.User.Username);
            if (receipt==null)
            {
                return this.Redirect("Receipt was not found");
            }
            var model = new ReceiptDetailsViewModel
            {
                Id=receipt.Id,
                IssuedOn=receipt.IssuedOn.ToString("dd/MM/yyyy"),
                DeliveryAddress=receipt.Package.ShippingAddress,
                Weight=receipt.Package.Weight.ToString("F2"),
                Description=receipt.Package.Description,
                Recepient = receipt.Recipient.Username,
                Total=receipt.Fee.ToString("F2")
            };

            return this.View(model);
        }
    }
}
