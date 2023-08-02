﻿// <auto-generated />
using CoachsBox.Coaching.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    [DbContext(typeof(CoachsBoxContext))]
    [Migration("20200112170251_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CoachsBox.Coaching.BranchModel.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ContactPersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContactPersonId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("CoachsBox.Coaching.CoachModel.Coach", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Coaches");
                });

            modelBuilder.Entity("CoachsBox.Coaching.GroupModel.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("CoachsBox.Coaching.PersonModel.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("CoachsBox.Coaching.ScheduleModel.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<int>("CoachId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("CoachId");

                    b.HasIndex("GroupId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("CoachsBox.Coaching.StudentModel.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("CoachsBox.Coaching.BranchModel.Branch", b =>
                {
                    b.HasOne("CoachsBox.Coaching.PersonModel.Person", "ContactPerson")
                        .WithMany()
                        .HasForeignKey("ContactPersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsMany("CoachsBox.Coaching.BranchModel.CoachingStaffMember", "CoachingStaff", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<int>("BranchId")
                                .HasColumnType("int");

                            b1.Property<int>("CoachId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("BranchId");

                            b1.HasIndex("CoachId")
                                .IsUnique();

                            b1.ToTable("CoachingStaffMember");

                            b1.WithOwner()
                                .HasForeignKey("BranchId");

                            b1.HasOne("CoachsBox.Coaching.CoachModel.Coach", "Coach")
                                .WithOne()
                                .HasForeignKey("CoachsBox.Coaching.BranchModel.CoachingStaffMember", "CoachId")
                                .OnDelete(DeleteBehavior.Restrict)
                                .IsRequired();
                        });

                    b.OwnsOne("CoachsBox.Coaching.Shared.Address", "Address", b1 =>
                        {
                            b1.Property<int>("BranchId")
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .HasColumnType("longtext");

                            b1.Property<string>("Country")
                                .HasColumnType("longtext");

                            b1.Property<string>("State")
                                .HasColumnType("longtext");

                            b1.Property<string>("Street")
                                .HasColumnType("longtext");

                            b1.Property<string>("ZipCode")
                                .HasColumnType("longtext");

                            b1.HasKey("BranchId");

                            b1.ToTable("Branches");

                            b1.WithOwner()
                                .HasForeignKey("BranchId");
                        });
                });

            modelBuilder.Entity("CoachsBox.Coaching.CoachModel.Coach", b =>
                {
                    b.HasOne("CoachsBox.Coaching.PersonModel.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoachsBox.Coaching.GroupModel.Group", b =>
                {
                    b.HasOne("CoachsBox.Coaching.BranchModel.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("CoachsBox.Coaching.GroupModel.EnrolledStudent", "EnrolledStudents", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<int>("GroupId")
                                .HasColumnType("int");

                            b1.Property<int>("StudentId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("GroupId");

                            b1.HasIndex("StudentId");

                            b1.ToTable("EnrolledStudent");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");

                            b1.HasOne("CoachsBox.Coaching.StudentModel.Student", "Student")
                                .WithMany()
                                .HasForeignKey("StudentId")
                                .OnDelete(DeleteBehavior.Restrict)
                                .IsRequired();
                        });

                    b.OwnsOne("CoachsBox.Coaching.GroupModel.Sport", "Sport", b1 =>
                        {
                            b1.Property<int>("GroupId")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .HasColumnType("longtext");

                            b1.HasKey("GroupId");

                            b1.ToTable("Groups");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");
                        });

                    b.OwnsOne("CoachsBox.Coaching.GroupModel.TrainingProgramSpecification", "TrainingProgramm", b1 =>
                        {
                            b1.Property<int>("GroupId")
                                .HasColumnType("int");

                            b1.Property<int>("MaximumAge")
                                .HasColumnType("int");

                            b1.Property<int>("MinimumAge")
                                .HasColumnType("int");

                            b1.HasKey("GroupId");

                            b1.ToTable("Groups");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");
                        });
                });

            modelBuilder.Entity("CoachsBox.Coaching.PersonModel.Person", b =>
                {
                    b.OwnsOne("CoachsBox.Coaching.PersonModel.Gender", "Gender", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .HasColumnType("int");

                            b1.Property<string>("Value")
                                .HasColumnType("longtext");

                            b1.HasKey("PersonId");

                            b1.ToTable("Persons");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.OwnsOne("CoachsBox.Coaching.PersonModel.PersonName", "Name", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .HasColumnType("longtext");

                            b1.Property<string>("Patronymic")
                                .HasColumnType("longtext");

                            b1.Property<string>("Surname")
                                .HasColumnType("longtext");

                            b1.HasKey("PersonId");

                            b1.ToTable("Persons");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.OwnsOne("CoachsBox.Coaching.PersonModel.PersonalInformation", "PersonalInformation", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .HasColumnType("int");

                            b1.HasKey("PersonId");

                            b1.ToTable("Persons");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");

                            b1.OwnsOne("CoachsBox.Coaching.PersonModel.EmailAddress", "Email", b2 =>
                                {
                                    b2.Property<int>("PersonalInformationPersonId")
                                        .HasColumnType("int");

                                    b2.Property<string>("Value")
                                        .HasColumnType("longtext");

                                    b2.HasKey("PersonalInformationPersonId");

                                    b2.ToTable("Persons");

                                    b2.WithOwner()
                                        .HasForeignKey("PersonalInformationPersonId");
                                });

                            b1.OwnsOne("CoachsBox.Coaching.PersonModel.PhoneNumber", "PhoneNumber", b2 =>
                                {
                                    b2.Property<int>("PersonalInformationPersonId")
                                        .HasColumnType("int");

                                    b2.Property<string>("Value")
                                        .HasColumnType("longtext");

                                    b2.HasKey("PersonalInformationPersonId");

                                    b2.ToTable("Persons");

                                    b2.WithOwner()
                                        .HasForeignKey("PersonalInformationPersonId");
                                });

                            b1.OwnsOne("CoachsBox.Coaching.Shared.Address", "Address", b2 =>
                                {
                                    b2.Property<int>("PersonalInformationPersonId")
                                        .HasColumnType("int");

                                    b2.Property<string>("City")
                                        .HasColumnType("longtext");

                                    b2.Property<string>("Country")
                                        .HasColumnType("longtext");

                                    b2.Property<string>("State")
                                        .HasColumnType("longtext");

                                    b2.Property<string>("Street")
                                        .HasColumnType("longtext");

                                    b2.Property<string>("ZipCode")
                                        .HasColumnType("longtext");

                                    b2.HasKey("PersonalInformationPersonId");

                                    b2.ToTable("Persons");

                                    b2.WithOwner()
                                        .HasForeignKey("PersonalInformationPersonId");
                                });
                        });

                    b.OwnsOne("CoachsBox.Coaching.Shared.Date", "Birthday", b1 =>
                        {
                            b1.Property<int>("PersonId")
                                .HasColumnType("int");

                            b1.Property<byte>("Day")
                                .HasColumnType("tinyint unsigned");

                            b1.Property<int>("Year")
                                .HasColumnType("int");

                            b1.HasKey("PersonId");

                            b1.ToTable("Persons");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");

                            b1.OwnsOne("CoachsBox.Coaching.Shared.Month", "Month", b2 =>
                                {
                                    b2.Property<int>("DatePersonId")
                                        .HasColumnType("int");

                                    b2.Property<string>("Name")
                                        .HasColumnType("longtext");

                                    b2.Property<int>("Number")
                                        .HasColumnType("int");

                                    b2.HasKey("DatePersonId");

                                    b2.ToTable("Persons");

                                    b2.WithOwner()
                                        .HasForeignKey("DatePersonId");
                                });
                        });
                });

            modelBuilder.Entity("CoachsBox.Coaching.ScheduleModel.Schedule", b =>
                {
                    b.HasOne("CoachsBox.Coaching.BranchModel.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CoachsBox.Coaching.CoachModel.Coach", "Coach")
                        .WithMany()
                        .HasForeignKey("CoachId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CoachsBox.Coaching.GroupModel.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("CoachsBox.Coaching.ScheduleModel.TrainingLocation", "TrainingLocation", b1 =>
                        {
                            b1.Property<int>("ScheduleId")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .HasColumnType("longtext");

                            b1.HasKey("ScheduleId");

                            b1.ToTable("Schedules");

                            b1.WithOwner()
                                .HasForeignKey("ScheduleId");

                            b1.OwnsOne("CoachsBox.Coaching.Shared.Address", "Address", b2 =>
                                {
                                    b2.Property<int>("TrainingLocationScheduleId")
                                        .HasColumnType("int");

                                    b2.Property<string>("City")
                                        .HasColumnType("longtext");

                                    b2.Property<string>("Country")
                                        .HasColumnType("longtext");

                                    b2.Property<string>("State")
                                        .HasColumnType("longtext");

                                    b2.Property<string>("Street")
                                        .HasColumnType("longtext");

                                    b2.Property<string>("ZipCode")
                                        .HasColumnType("longtext");

                                    b2.HasKey("TrainingLocationScheduleId");

                                    b2.ToTable("Schedules");

                                    b2.WithOwner()
                                        .HasForeignKey("TrainingLocationScheduleId");
                                });
                        });

                    b.OwnsMany("CoachsBox.Coaching.ScheduleModel.TrainingTime", "TrainingList", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<int>("DayOfWeek")
                                .HasColumnType("int");

                            b1.Property<int>("ScheduleId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("ScheduleId");

                            b1.ToTable("TrainingTime");

                            b1.WithOwner()
                                .HasForeignKey("ScheduleId");

                            b1.OwnsOne("CoachsBox.Coaching.Shared.Date", "Date", b2 =>
                                {
                                    b2.Property<int>("TrainingTimeId")
                                        .HasColumnType("int");

                                    b2.Property<byte>("Day")
                                        .HasColumnType("tinyint unsigned");

                                    b2.Property<int>("Year")
                                        .HasColumnType("int");

                                    b2.HasKey("TrainingTimeId");

                                    b2.ToTable("TrainingTime");

                                    b2.WithOwner()
                                        .HasForeignKey("TrainingTimeId");

                                    b2.OwnsOne("CoachsBox.Coaching.Shared.Month", "Month", b3 =>
                                        {
                                            b3.Property<int>("DateTrainingTimeId")
                                                .HasColumnType("int");

                                            b3.Property<string>("Name")
                                                .HasColumnType("longtext");

                                            b3.Property<int>("Number")
                                                .HasColumnType("int");

                                            b3.HasKey("DateTrainingTimeId");

                                            b3.ToTable("TrainingTime");

                                            b3.WithOwner()
                                                .HasForeignKey("DateTrainingTimeId");
                                        });
                                });

                            b1.OwnsOne("CoachsBox.Coaching.Shared.TimeOfDay", "End", b2 =>
                                {
                                    b2.Property<int>("TrainingTimeId")
                                        .HasColumnType("int");

                                    b2.Property<byte>("Hours")
                                        .HasColumnType("tinyint unsigned");

                                    b2.Property<byte>("Minutes")
                                        .HasColumnType("tinyint unsigned");

                                    b2.HasKey("TrainingTimeId");

                                    b2.ToTable("TrainingTime");

                                    b2.WithOwner()
                                        .HasForeignKey("TrainingTimeId");
                                });

                            b1.OwnsOne("CoachsBox.Coaching.Shared.TimeOfDay", "Start", b2 =>
                                {
                                    b2.Property<int>("TrainingTimeId")
                                        .HasColumnType("int");

                                    b2.Property<byte>("Hours")
                                        .HasColumnType("tinyint unsigned");

                                    b2.Property<byte>("Minutes")
                                        .HasColumnType("tinyint unsigned");

                                    b2.HasKey("TrainingTimeId");

                                    b2.ToTable("TrainingTime");

                                    b2.WithOwner()
                                        .HasForeignKey("TrainingTimeId");
                                });
                        });
                });

            modelBuilder.Entity("CoachsBox.Coaching.StudentModel.Student", b =>
                {
                    b.HasOne("CoachsBox.Coaching.PersonModel.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
