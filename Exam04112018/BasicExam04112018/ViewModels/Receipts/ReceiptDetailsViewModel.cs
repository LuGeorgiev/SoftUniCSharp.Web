using System;
using System.Collections.Generic;
using System.Text;

namespace BasicExam04112018.ViewModels.Receipts
{
    public class ReceiptDetailsViewModel
    {
        public int Id { get; set; }

        public string DeliveryAddress { get; set; }

        public string IssuedOn { get; set; }

        public string Weight { get; set; }

        public string Description { get; set; }

        public string Recepient { get; set; }

        public string Total { get; set; }
    }
}

