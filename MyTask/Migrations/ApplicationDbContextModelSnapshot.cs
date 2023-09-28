﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyTask;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyTask.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyTask.Assignment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DueTo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Assignment");
                });

            modelBuilder.Entity("MyTask.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("ProfilePhotoUrl")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MyTask.UserAssignment", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AssignmentId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "AssignmentId");

                    b.HasIndex("AssignmentId");

                    b.ToTable("UserAssignment");
                });

            modelBuilder.Entity("MyTask.Assignment", b =>
                {
                    b.HasOne("MyTask.User", "Creator")
                        .WithMany("Assignment")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("MyTask.User", b =>
                {
                    b.OwnsOne("MyTask.RefreshToken", "RefreshToken", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<DateTime>("ExpireAt")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("character varying(128)");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("RefreshToken")
                        .IsRequired();
                });

            modelBuilder.Entity("MyTask.UserAssignment", b =>
                {
                    b.HasOne("MyTask.Assignment", "Assignment")
                        .WithMany("UserAssignment")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyTask.User", "User")
                        .WithMany("UserAssignment")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyTask.Assignment", b =>
                {
                    b.Navigation("UserAssignment");
                });

            modelBuilder.Entity("MyTask.User", b =>
                {
                    b.Navigation("Assignment");

                    b.Navigation("UserAssignment");
                });
#pragma warning restore 612, 618
        }
    }
}