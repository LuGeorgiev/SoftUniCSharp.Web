using Chushka.ViewModels.Orders;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System.Linq;

namespace Chushka.Controllers
{
    public class OrdersController:BaseController
    {
        [Authorize("Admin")]
        [HttpGet]
        public IHttpResponse All()
        {
            var orders = this.db.Orders
                .Select(x => new OrderModel
                {
                    Id = x.Id,
                    CustomerName = x.Client.Username,
                    ProductName = x.Product.Name,
                    OrderedOn = x.OrderedOn.ToLongTimeString()
                })
                .ToList();
            var model = new OrderesListingModel()
            {
                Orders = orders
            };

            return this.View(model);
        }
    }
}
