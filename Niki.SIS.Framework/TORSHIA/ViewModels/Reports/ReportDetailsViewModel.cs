using System;
using System.Collections.Generic;
using System.Text;

namespace TORSHIA.ViewModels.Reports
{
    public class ReportDetailsViewModel
    {
        public int Id { get; set; }

        public string  TaskTitle { get; set; }

        public int Level { get; set; }

        public string DueDate { get; set; }

        public string ReportedDate { get; set; }

        public string ReportedBy { get; set; }

        public string  Participants { get; set; }

        public string AffectedSectors { get; set; }

        public string  Description { get; set; }

        public string Status { get; set; }
    }
}
