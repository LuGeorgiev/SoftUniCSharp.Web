using System;
using System.Collections.Generic;
using System.Text;

namespace Chushka.ViewModels.Orders
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string ProductName { get; set; }

        public string OrderedOn { get; set; }
    }
}
