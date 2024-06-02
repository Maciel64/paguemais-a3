﻿// <auto-generated />
using System;
using Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PagueMais.Migrations
{
    [DbContext(typeof(Database))]
    [Migration("20240601225038_CreatePurchaseTable")]
    partial class CreatePurchaseTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entities.Purchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Total")
                        .HasColumnType("float");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FinishedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>(" PaymentMethod")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Purchase");
                });
#pragma warning restore 612, 618
        }
    }
}
