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

        public DbSet<Package> Packages { get; set; }

        public DbSet<Receit> Receits { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=LUBO-NB\\SQLEXPRESS;Database=Panda;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(x => x.Packages)
                .WithOne(x => x.Recepient)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(x => x.Receits)
                .WithOne(x => x.Recipient)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
