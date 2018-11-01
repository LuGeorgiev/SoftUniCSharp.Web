using System;
using System.Collections.Generic;
using System.Text;

namespace Chushka.ViewModels.Orders
{
    public class OrderesListingModel
    {
        public IEnumerable<OrderModel> Orders { get; set; }
    }
}
