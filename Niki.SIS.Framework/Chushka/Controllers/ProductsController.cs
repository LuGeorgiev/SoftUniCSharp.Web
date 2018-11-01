using Chushka.Models;
using Chushka.Models.Enums;
using Chushka.ViewModels.Products;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chushka.Controllers
{
    public class ProductsController : BaseController
    {
        [Authorize]
        [HttpGet("/products/details")]
        public IHttpResponse Details(int id)
        {
            var product = this.db
                .Products
                .FirstOrDefault(x => x.Id == id);
            if (product==null)
            {
                return this.BadRequestError("Product was not found in database");
            }

            var model = new ProductDetailsViewModel
            {
                Name=product.Name,
                Description=product.Description,
                Id=product.Id,
                Price=product.Price.ToString("F2"),
                Type=product.Type.ToString()
            };

            return this.View(model);
        }

        [Authorize]
        [HttpGet("/products/order")]
        public IHttpResponse Order(int id)
        {
            var product = this.db
                .Products
                .FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Product was not found in database");
            }

            var user = this.db
                .Users
                .FirstOrDefault(x => x.Username == this.User.Username);
            if (user==null)
            {
                return this.Redirect("/users/login");
            }

            user.Orders.Add(new Order
            {
                ProductId=product.Id,
                OrderedOn = DateTime.Now
            });
            db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        [HttpGet("/products/edit")]
        public IHttpResponse Edit(int id)
        {
            var product = this.db
               .Products
               .FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Product was not found in database");
            }

            var model = new ProducteditViewModel
            {
                Name=product.Name,
                Description =product.Description,
                Id=product.Id,
                Price=product.Price.ToString("F2")
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost("/products/edit")]
        public IHttpResponse Edit(ProductEditedModel model)
        {
            var product = this.db
               .Products
               .FirstOrDefault(x => x.Id == model.Id);
            if (product == null)
            {
                return this.BadRequestError("Product was not found in the database");
            }

            if (!Enum.TryParse<ProductType>(model.Type,true, out var type))
            {
                return this.BadRequestError($"Product Type: {model.Type} is not correct");
            }
            if (!decimal.TryParse(model.Price, out var price))
            {
                return this.BadRequestError($"Product Price: {model.Price} is not correct");
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = price;
            product.Type = type;

            db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        [HttpGet("/products/delete")]
        public IHttpResponse Delete(int id)
        {
            var product = this.db
              .Products
              .FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Product was not found in database");
            }

            var model = new ProducteditViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Id = product.Id,
                Price = product.Price.ToString("F2")
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpGet("/products/todelete")]
        public IHttpResponse ToDelete(int id)
        {
            var product = this.db
            .Products
            .FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Product was not found in database");
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        [HttpGet]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(ProductCreateViewModel model)
        {
            if (!Enum.TryParse<ProductType>(model.Type, true, out var type))
            {
                return this.BadRequestError($"Product Type: {model.Type} is not correct");
            }
            if (!decimal.TryParse(model.Price, out var price))
            {
                return this.BadRequestError($"Product Price: {model.Price} is not correct");
            }
            var product = new Product
            {
                Name=model.Name,
                Price = price,
                Type = type,
                Description=model.Description
            };

            this.db.Products.Add(product);
            this.db.SaveChanges();

            return this.View();
        }

    }
}
