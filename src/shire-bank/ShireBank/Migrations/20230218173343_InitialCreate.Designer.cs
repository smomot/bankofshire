﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedComponents;
using ShireBankService.Infrastructure;

#nullable disable

namespace ShireBankService.Migrations
{
    [DbContext(typeof(BankDbContext))]
    [Migration("20230218173343_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("ShireBankService.Infrastructure.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Ballance")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("ClosedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<float>("DebtLimit")
                        .HasColumnType("REAL");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ShireBankService.Infrastructure.AccountOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("Ammount")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("OperatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountOperations");
                });

            modelBuilder.Entity("ShireBankService.Infrastructure.AccountOperation", b =>
                {
                    b.HasOne("ShireBankService.Infrastructure.Account", "Account")
                        .WithMany("AccountOperations")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ShireBankService.Infrastructure.Account", b =>
                {
                    b.Navigation("AccountOperations");
                });
#pragma warning restore 612, 618
        }
    }
}
