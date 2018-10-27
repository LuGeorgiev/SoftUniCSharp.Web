using System;
using System.Collections.Generic;
using System.Text;

namespace TORSHIA.ViewModels.Tasks
{
    public class CreateTaskViewModel
    {
        public string Title { get; set; }

        public DateTime? DueDate { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public string Customers { get; set; }

        public string Marketing { get; set; }

        public string Finances { get; set; }

        public string Internal { get; set; }

        public string Management { get; set; }

    }
}
