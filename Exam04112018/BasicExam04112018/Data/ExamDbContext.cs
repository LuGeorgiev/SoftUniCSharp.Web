using BasicExam04112018.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicExam04112018.Data
{
    public class ExamDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=LUBO-NB\\SQLEXPRESS;Database=ExamBasic;Integrated Security=True;");
        }
    }
}
