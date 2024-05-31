﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using agile_dev.Repo;

#nullable disable

namespace agile_dev.Migrations
{
    [DbContext(typeof(InitContext))]
    [Migration("20240531073056_test")]
    partial class test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("agile_dev.Models.Allergy", b =>
                {
                    b.Property<int>("AllergyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.HasKey("AllergyId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Allergy");
                });

            modelBuilder.Entity("agile_dev.Models.ContactPerson", b =>
                {
                    b.Property<int>("ContactPersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Number")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("ContactPersonId");

                    b.ToTable("ContactPerson");
                });

            modelBuilder.Entity("agile_dev.Models.CustomField", b =>
                {
                    b.Property<int>("CustomFieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<bool>("Value")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("CustomFieldId");

                    b.ToTable("CustomField");
                });

            modelBuilder.Entity("agile_dev.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("ContactPersonId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("varchar(3000)");

                    b.Property<int>("EventDateTimeId")
                        .HasColumnType("int");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<int>("PlaceId")
                        .HasColumnType("int");

                    b.Property<bool>("Published")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("EventId");

                    b.HasIndex("ContactPersonId");

                    b.HasIndex("EventDateTimeId")
                        .IsUnique();

                    b.HasIndex("ImageId");

                    b.HasIndex("PlaceId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("agile_dev.Models.EventCustomField", b =>
                {
                    b.Property<int>("EventCustomFieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CustomFieldId")
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.HasKey("EventCustomFieldId");

                    b.HasIndex("CustomFieldId");

                    b.HasIndex("EventId");

                    b.ToTable("EventCustomField");
                });

            modelBuilder.Entity("agile_dev.Models.EventDateTime", b =>
                {
                    b.Property<int>("EventDateTimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("EventDateTimeId");

                    b.ToTable("EventDateTime");
                });

            modelBuilder.Entity("agile_dev.Models.Follower", b =>
                {
                    b.Property<int>("FollowerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("FollowerId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("UserId");

                    b.ToTable("Follower");
                });

            modelBuilder.Entity("agile_dev.Models.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.HasKey("ImageId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("agile_dev.Models.Notice", b =>
                {
                    b.Property<int>("NoticeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Expire")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("Valid")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("NoticeId");

                    b.HasIndex("UserId");

                    b.ToTable("Notice");
                });

            modelBuilder.Entity("agile_dev.Models.Organisator", b =>
                {
                    b.Property<int>("OrganisatorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrganisatorId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("UserId");

                    b.ToTable("Organisator");
                });

            modelBuilder.Entity("agile_dev.Models.Organization", b =>
                {
                    b.Property<int>("OrganizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("OrganizationId");

                    b.HasIndex("ImageId");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("agile_dev.Models.Place", b =>
                {
                    b.Property<int>("PlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.HasKey("PlaceId");

                    b.ToTable("Place");
                });

            modelBuilder.Entity("agile_dev.Models.Profile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ExtraInfo")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("ProfileId");

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("agile_dev.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Admin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int?>("ProfileId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("ProfileId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("agile_dev.Models.UserEvent", b =>
                {
                    b.Property<int>("UserEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("QueueNumber")
                        .HasColumnType("int");

                    b.Property<bool>("Used")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserEventId");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("UserEvent");
                });

            modelBuilder.Entity("agile_dev.Models.Allergy", b =>
                {
                    b.HasOne("agile_dev.Models.Profile", "Profile")
                        .WithMany("Allergies")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("agile_dev.Models.Event", b =>
                {
                    b.HasOne("agile_dev.Models.ContactPerson", "ContactPerson")
                        .WithMany("Events")
                        .HasForeignKey("ContactPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("agile_dev.Models.EventDateTime", "EventDateTime")
                        .WithOne("Event")
                        .HasForeignKey("agile_dev.Models.Event", "EventDateTimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("agile_dev.Models.Image", "Image")
                        .WithMany("Events")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("agile_dev.Models.Place", "Place")
                        .WithMany("Events")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactPerson");

                    b.Navigation("EventDateTime");

                    b.Navigation("Image");

                    b.Navigation("Place");
                });

            modelBuilder.Entity("agile_dev.Models.EventCustomField", b =>
                {
                    b.HasOne("agile_dev.Models.CustomField", "CustomField")
                        .WithMany("EventCustomFields")
                        .HasForeignKey("CustomFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("agile_dev.Models.Event", "Event")
                        .WithMany("EventCustomFields")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomField");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("agile_dev.Models.Follower", b =>
                {
                    b.HasOne("agile_dev.Models.Organization", "Organization")
                        .WithMany("Followers")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("agile_dev.Models.User", "User")
                        .WithMany("FollowOrganization")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("agile_dev.Models.Notice", b =>
                {
                    b.HasOne("agile_dev.Models.User", "User")
                        .WithMany("Notices")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("agile_dev.Models.Organisator", b =>
                {
                    b.HasOne("agile_dev.Models.Organization", "Organization")
                        .WithMany("Organisators")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("agile_dev.Models.User", "User")
                        .WithMany("OrganisatorOrganization")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("agile_dev.Models.Organization", b =>
                {
                    b.HasOne("agile_dev.Models.Image", "Image")
                        .WithMany("Organizations")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");
                });

            modelBuilder.Entity("agile_dev.Models.User", b =>
                {
                    b.HasOne("agile_dev.Models.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("agile_dev.Models.UserEvent", b =>
                {
                    b.HasOne("agile_dev.Models.Event", "Event")
                        .WithMany("UserEvents")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("agile_dev.Models.User", "User")
                        .WithMany("UserEvents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("agile_dev.Models.ContactPerson", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("agile_dev.Models.CustomField", b =>
                {
                    b.Navigation("EventCustomFields");
                });

            modelBuilder.Entity("agile_dev.Models.Event", b =>
                {
                    b.Navigation("EventCustomFields");

                    b.Navigation("UserEvents");
                });

            modelBuilder.Entity("agile_dev.Models.EventDateTime", b =>
                {
                    b.Navigation("Event")
                        .IsRequired();
                });

            modelBuilder.Entity("agile_dev.Models.Image", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Organizations");
                });

            modelBuilder.Entity("agile_dev.Models.Organization", b =>
                {
                    b.Navigation("Followers");

                    b.Navigation("Organisators");
                });

            modelBuilder.Entity("agile_dev.Models.Place", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("agile_dev.Models.Profile", b =>
                {
                    b.Navigation("Allergies");
                });

            modelBuilder.Entity("agile_dev.Models.User", b =>
                {
                    b.Navigation("FollowOrganization");

                    b.Navigation("Notices");

                    b.Navigation("OrganisatorOrganization");

                    b.Navigation("UserEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
