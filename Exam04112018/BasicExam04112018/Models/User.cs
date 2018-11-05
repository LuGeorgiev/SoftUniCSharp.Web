using System;
using System.Collections.Generic;
using System.Text;

namespace BasicExam04112018.Models
{
    public class User
    {
        public User()
        {
            this.Packages = new HashSet<Package>();
            this.Receits = new HashSet<Receit>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

        public virtual ICollection<Package> Packages { get; set; }

        public virtual ICollection<Receit> Receits { get; set; }
    }
}
