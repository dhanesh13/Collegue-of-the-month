﻿// <auto-generated />
using Colleague_Of_The_Month.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Colleague_Of_The_Month.Migrations
{
    [DbContext(typeof(COTMDBContext))]
    partial class COTMDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Colleague_Of_The_Month.Models.BusinessUnit", b =>
                {
                    b.Property<int>("BusinessUnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("BusinessUnitId");

                    b.ToTable("BusinessUnits");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.CostCentre", b =>
                {
                    b.Property<int>("CostCentreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CostCentreCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CostCentreName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CostCentreId");

                    b.ToTable("CostCentres");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.Division", b =>
                {
                    b.Property<int>("DivisionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("DivisionId");

                    b.ToTable("Divisions");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.Elected", b =>
                {
                    b.Property<int>("ElectedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.HasKey("ElectedId");

                    b.ToTable("Electeds");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BusinessUnitId")
                        .HasColumnType("int");

                    b.Property<int>("CostCentreId")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("DivisionId")
                        .HasColumnType("int");

                    b.Property<int>("PayrollId")
                        .HasColumnType("int");

                    b.Property<int>("SubdivisionId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId");

                    b.HasIndex("BusinessUnitId");

                    b.HasIndex("CostCentreId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("DivisionId");

                    b.HasIndex("PayrollId");

                    b.HasIndex("SubdivisionId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.EmployeeDetail", b =>
                {
                    b.Property<int>("PayrollId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Manager")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PreferredName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PayrollId");

                    b.ToTable("EmployeeDetails");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.Nominee", b =>
                {
                    b.Property<int>("NomineeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("NomineeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NomineeId");

                    b.ToTable("Nominees");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.Subdivision", b =>
                {
                    b.Property<int>("SubdivisionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("SubdivisionId");

                    b.ToTable("Subdivisions");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.Voter", b =>
                {
                    b.Property<int>("VoterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("VoterName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VoterId");

                    b.ToTable("Voters");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.VotingForm", b =>
                {
                    b.Property<int>("VotingFormId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Division")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Impact")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Manager")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NomineeFullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Values")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("VotingFormId");

                    b.ToTable("VotingForms");
                });

            modelBuilder.Entity("Colleague_Of_The_Month.Models.Employee", b =>
                {
                    b.HasOne("Colleague_Of_The_Month.Models.BusinessUnit", "BusinessUnit")
                        .WithMany()
                        .HasForeignKey("BusinessUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Colleague_Of_The_Month.Models.CostCentre", "CostCentre")
                        .WithMany()
                        .HasForeignKey("CostCentreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Colleague_Of_The_Month.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Colleague_Of_The_Month.Models.Division", "Division")
                        .WithMany()
                        .HasForeignKey("DivisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Colleague_Of_The_Month.Models.EmployeeDetail", "EmployeeDetail")
                        .WithMany()
                        .HasForeignKey("PayrollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Colleague_Of_The_Month.Models.Subdivision", "Subdivision")
                        .WithMany()
                        .HasForeignKey("SubdivisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusinessUnit");

                    b.Navigation("CostCentre");

                    b.Navigation("Department");

                    b.Navigation("Division");

                    b.Navigation("EmployeeDetail");

                    b.Navigation("Subdivision");
                });
#pragma warning restore 612, 618
        }
    }
}
