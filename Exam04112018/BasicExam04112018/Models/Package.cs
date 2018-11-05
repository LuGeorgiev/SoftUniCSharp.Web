using System;
using System.Collections.Generic;
using System.Text;

namespace BasicExam04112018.Models
{
    public class Package
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public float Weight { get; set; }

        public string ShippingAddress { get; set; }

        public Status Status { get; set; }

        public DateTime? EstemetedDeliveryDate { get; set; }

        public int RecepientId { get; set; }

        public virtual User Recepient { get; set; }

    }
}
