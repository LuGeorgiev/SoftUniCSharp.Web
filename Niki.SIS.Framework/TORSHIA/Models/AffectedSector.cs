using System;
using System.Collections.Generic;
using System.Text;
using TORSHIA.Models.Enums;

namespace TORSHIA.Models
{
    public class AffectedSector
    {
        public int Id { get; set; }

        public Sector Sector { get; set; }

        public int TaskId { get; set; }

        public virtual Task Task { get; set; }
    }
}
