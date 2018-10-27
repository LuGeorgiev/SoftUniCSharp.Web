using System;
using System.Collections.Generic;
using System.Text;
using TORSHIA.Models.Enums;

namespace TORSHIA.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsReported { get; set; } = false;

        public string Description { get; set; }

        public string Participants { get; set; }

        public virtual ICollection<AffectedSector> AffectedSectors { get; set; } = new List<AffectedSector>();

        public int? UserId { get; set; }

        public virtual User User { get; set; }

    }
}
