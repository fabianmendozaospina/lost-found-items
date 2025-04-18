﻿// <auto-generated />
using System;
using LostAndFoundItems.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LostAndFoundItems.DAL.Migrations
{
    [DbContext(typeof(LostAndFoundDbContext))]
    [Migration("20250411213014_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LostAndFoundItems.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("CategoryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.ClaimRequest", b =>
                {
                    b.Property<int>("ClaimRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClaimRequestId"));

                    b.Property<int>("ClaimStatusId")
                        .HasColumnType("int");

                    b.Property<int>("ClaimingUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("FoundItemId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClaimRequestId");

                    b.HasIndex("ClaimStatusId");

                    b.HasIndex("ClaimingUserId");

                    b.HasIndex("FoundItemId", "ClaimingUserId")
                        .IsUnique();

                    b.ToTable("ClaimRequests");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.ClaimStatus", b =>
                {
                    b.Property<int>("ClaimStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClaimStatusId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("ClaimStatusId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ClaimStatuses");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.FoundItem", b =>
                {
                    b.Property<int>("FoundItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FoundItemId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("FoundDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("FoundItemId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("FoundItems");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("LocationId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.LostItem", b =>
                {
                    b.Property<int>("LostItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LostItemId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LostDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LostItemId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("LostItems");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.MatchItem", b =>
                {
                    b.Property<int>("MatchItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MatchItemId"));

                    b.Property<int>("FoundItemId")
                        .HasColumnType("int");

                    b.Property<int>("LostItemId")
                        .HasColumnType("int");

                    b.Property<int>("MatchStatusId")
                        .HasColumnType("int");

                    b.Property<int>("MatchUserId")
                        .HasColumnType("int");

                    b.Property<string>("Observation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MatchItemId");

                    b.HasIndex("FoundItemId")
                        .IsUnique();

                    b.HasIndex("LostItemId")
                        .IsUnique();

                    b.HasIndex("MatchStatusId");

                    b.HasIndex("MatchUserId");

                    b.HasIndex("FoundItemId", "LostItemId")
                        .IsUnique();

                    b.ToTable("MatchItems");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.MatchStatus", b =>
                {
                    b.Property<int>("MatchStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MatchStatusId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("MatchStatusId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MatchStatuses");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("RoleId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.ClaimRequest", b =>
                {
                    b.HasOne("LostAndFoundItems.Models.ClaimStatus", "ClaimStatus")
                        .WithMany("ClaimRequests")
                        .HasForeignKey("ClaimStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.User", "User")
                        .WithMany("ClaimRequests")
                        .HasForeignKey("ClaimingUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.FoundItem", "FoundItem")
                        .WithMany("ClaimRequests")
                        .HasForeignKey("FoundItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClaimStatus");

                    b.Navigation("FoundItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.FoundItem", b =>
                {
                    b.HasOne("LostAndFoundItems.Models.Category", "Category")
                        .WithMany("FoundItems")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.Location", "Location")
                        .WithMany("FoundItems")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Location");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.LostItem", b =>
                {
                    b.HasOne("LostAndFoundItems.Models.Category", "Category")
                        .WithMany("LostItems")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.Location", "Location")
                        .WithMany("LostItems")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.User", "User")
                        .WithMany("LostItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Location");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.MatchItem", b =>
                {
                    b.HasOne("LostAndFoundItems.Models.FoundItem", "FoundItem")
                        .WithOne("MatchItem")
                        .HasForeignKey("LostAndFoundItems.Models.MatchItem", "FoundItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.LostItem", "LostItem")
                        .WithOne("MatchItem")
                        .HasForeignKey("LostAndFoundItems.Models.MatchItem", "LostItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.MatchStatus", "MatchStatus")
                        .WithMany("MatchItems")
                        .HasForeignKey("MatchStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LostAndFoundItems.Models.User", "User")
                        .WithMany("MatchItems")
                        .HasForeignKey("MatchUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoundItem");

                    b.Navigation("LostItem");

                    b.Navigation("MatchStatus");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.User", b =>
                {
                    b.HasOne("LostAndFoundItems.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.Category", b =>
                {
                    b.Navigation("FoundItems");

                    b.Navigation("LostItems");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.ClaimStatus", b =>
                {
                    b.Navigation("ClaimRequests");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.FoundItem", b =>
                {
                    b.Navigation("ClaimRequests");

                    b.Navigation("MatchItem")
                        .IsRequired();
                });

            modelBuilder.Entity("LostAndFoundItems.Models.Location", b =>
                {
                    b.Navigation("FoundItems");

                    b.Navigation("LostItems");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.LostItem", b =>
                {
                    b.Navigation("MatchItem")
                        .IsRequired();
                });

            modelBuilder.Entity("LostAndFoundItems.Models.MatchStatus", b =>
                {
                    b.Navigation("MatchItems");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("LostAndFoundItems.Models.User", b =>
                {
                    b.Navigation("ClaimRequests");

                    b.Navigation("LostItems");

                    b.Navigation("MatchItems");
                });
#pragma warning restore 612, 618
        }
    }
}
