﻿// <auto-generated />
using System;
using EmployeeRestApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmployeeRestApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EmployeeRestApiLibrary.Models.Address", b =>
                {
                    b.Property<long>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("AddressId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("EmployeeId")
                        .HasColumnType("bigint");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AddressId");

                    b.HasIndex("EmployeeId")
                        .IsUnique();

                    b.ToTable("Address", (string)null);
                });

            modelBuilder.Entity("EmployeeRestApiLibrary.Models.Employee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("CurrentSalary")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("EmploymentCommencementDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("ManagerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("EmployeeRestApiLibrary.Models.Address", b =>
                {
                    b.HasOne("EmployeeRestApiLibrary.Models.Employee", "Employee")
                        .WithOne("HomeAddress")
                        .HasForeignKey("EmployeeRestApiLibrary.Models.Address", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EmployeeRestApiLibrary.Models.Employee", b =>
                {
                    b.HasOne("EmployeeRestApiLibrary.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("EmployeeRestApiLibrary.Models.Employee", b =>
                {
                    b.Navigation("HomeAddress")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}