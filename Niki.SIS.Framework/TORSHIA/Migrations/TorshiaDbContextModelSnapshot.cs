﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TORSHIA.Data;

namespace TORSHIA.Migrations
{
    [DbContext(typeof(TorshiaDbContext))]
    partial class TorshiaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TORSHIA.Models.AffectedSector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Sector");

                    b.Property<int>("TaskId");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("AffectedSectors");
                });

            modelBuilder.Entity("TORSHIA.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ReportedOn");

                    b.Property<int>("ReporterId");

                    b.Property<int>("Status");

                    b.Property<int>("TaskId");

                    b.HasKey("Id");

                    b.HasIndex("ReporterId");

                    b.HasIndex("TaskId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("TORSHIA.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<DateTime?>("DueDate");

                    b.Property<bool>("IsReported");

                    b.Property<string>("Participants");

                    b.Property<string>("Title");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TORSHIA.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.Property<int>("Role");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TORSHIA.Models.AffectedSector", b =>
                {
                    b.HasOne("TORSHIA.Models.Task", "Task")
                        .WithMany("AffectedSectors")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TORSHIA.Models.Report", b =>
                {
                    b.HasOne("TORSHIA.Models.User", "Reporter")
                        .WithMany()
                        .HasForeignKey("ReporterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TORSHIA.Models.Task", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TORSHIA.Models.Task", b =>
                {
                    b.HasOne("TORSHIA.Models.User", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
