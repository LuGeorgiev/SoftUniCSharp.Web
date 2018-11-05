using System;
using System.Collections.Generic;
using System.Text;

namespace BasicExam04112018.ViewModels.Receipts
{
    public class ReceitViewModel
    {
        public int Id { get; set; }

        public string Fee { get; set; }

        public string IssuedOn { get; set; }

        public string Recepient { get; set; }
    }
}
