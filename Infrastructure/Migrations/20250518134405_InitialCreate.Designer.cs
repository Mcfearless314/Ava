﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250518134405_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.4");

            modelBuilder.Entity("Infrastructure.Models.Credentials", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("Infrastructure.Models.Organisation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("AdminUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AdminUserId")
                        .IsUnique();

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("Infrastructure.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subtitle")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Infrastructure.Models.ProjectTask", b =>
                {
                    b.Property<Guid>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .HasMaxLength(4)
                        .HasColumnType("TEXT");

                    b.Property<string>("Body")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("ProjectId", "Id");

                    b.ToTable("ProjectTasks");
                });

            modelBuilder.Entity("Infrastructure.Models.ProjectUser", b =>
                {
                    b.Property<Guid>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProjectId", "UserId");

                    b.ToTable("ProjectUsers");
                });

            modelBuilder.Entity("Infrastructure.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OrganisationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Infrastructure.Models.UserActionsLog", b =>
                {
                    b.Property<Guid>("ActionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("ActionLog")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ActionPerformedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ActionUser")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OrganisationId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProjectTaskId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TargetUser")
                        .HasColumnType("TEXT");

                    b.HasKey("ActionId");

                    b.ToTable("UserActionsLogs");
                });

            modelBuilder.Entity("Infrastructure.Models.Credentials", b =>
                {
                    b.HasOne("Infrastructure.Models.User", null)
                        .WithOne("Credentials")
                        .HasForeignKey("Infrastructure.Models.Credentials", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.Organisation", b =>
                {
                    b.HasOne("Infrastructure.Models.User", "AdminUser")
                        .WithOne()
                        .HasForeignKey("Infrastructure.Models.Organisation", "AdminUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("AdminUser");
                });

            modelBuilder.Entity("Infrastructure.Models.Project", b =>
                {
                    b.HasOne("Infrastructure.Models.Organisation", null)
                        .WithMany("Projects")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.ProjectTask", b =>
                {
                    b.HasOne("Infrastructure.Models.Project", null)
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.ProjectUser", b =>
                {
                    b.HasOne("Infrastructure.Models.Project", null)
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Models.User", b =>
                {
                    b.HasOne("Infrastructure.Models.Organisation", null)
                        .WithMany("Users")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("Infrastructure.Models.Organisation", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Infrastructure.Models.Project", b =>
                {
                    b.Navigation("ProjectUsers");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Infrastructure.Models.User", b =>
                {
                    b.Navigation("Credentials")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
