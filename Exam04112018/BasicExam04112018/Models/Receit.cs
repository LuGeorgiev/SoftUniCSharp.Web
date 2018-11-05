﻿using System;

namespace BasicExam04112018.Models
{
    public class Receit
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; }

        public int RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        public int PackageId { get; set; }

        public virtual Package Package { get; set; }

    }
}
