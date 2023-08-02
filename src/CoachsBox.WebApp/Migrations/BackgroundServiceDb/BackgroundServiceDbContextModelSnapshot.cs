﻿// <auto-generated />
using System;
using CoachsBox.WebApp.Jobs.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoachsBox.WebApp.Migrations.BackgroundServiceDb
{
    [DbContext(typeof(BackgroundServiceDbContext))]
    partial class BackgroundServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CoachsBox.WebApp.Jobs.Data.ServiceEvent", b =>
                {
                    b.Property<string>("ServiceId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("UtcLastRun")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FailureReason")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.HasKey("ServiceId", "UtcLastRun");

                    b.ToTable("ServiceEvents");
                });

            modelBuilder.Entity("CoachsBox.WebApp.Jobs.Data.ServiceInfo", b =>
                {
                    b.Property<string>("ServiceId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("NextStart")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ServiceId");

                    b.ToTable("ServiceInfos");
                });

            modelBuilder.Entity("CoachsBox.WebApp.Jobs.Data.ServiceEvent", b =>
                {
                    b.HasOne("CoachsBox.WebApp.Jobs.Data.ServiceInfo", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}