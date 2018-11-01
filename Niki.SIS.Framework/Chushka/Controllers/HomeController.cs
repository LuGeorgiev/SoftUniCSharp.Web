using Chushka.ViewModels.Home;
using SIS.HTTP.Responses;
using System.Linq;

namespace Chushka.Controllers
{
    public class HomeController :BaseController
    {
        public IHttpResponse Index()
        {
            var products = this.db.Products
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price.ToString("F2"),
                    Description = x.Description.Length > 50 ?
                    x.Description.Substring(0, 50) + "..." :
                    x.Description
                })
                .ToList();

            var model = new ProductListingModel { Products = products };

            
            return View(model);
        }
    }
}
