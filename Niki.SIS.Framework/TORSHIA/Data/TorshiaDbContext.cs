using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TORSHIA.Models;

namespace TORSHIA.Data
{
    public class TorshiaDbContext : DbContext
    {
        public TorshiaDbContext()
        {
        }
        public TorshiaDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<AffectedSector> AffectedSectors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Torshia; Integrated Security=True;");
        }
    }
}
