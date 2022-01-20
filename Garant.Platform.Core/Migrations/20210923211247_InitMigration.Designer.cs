﻿// <auto-generated />
using System;
using Garant.Platform.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Garant.Platform.Core.Migrations
{
    [DbContext(typeof(PostgreDbContext))]
    [Migration("20210923211247_InitMigration")]
    partial class InitMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Garant.Platform.Models.Entities.User.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("City")
                        .HasColumnType("varchar(200)")
                        .HasColumnName("City");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp")
                        .HasColumnName("DateRegister");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("LastName");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Patronymic");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("RememberMe")
                        .HasColumnType("boolean")
                        .HasColumnName("RememberMe");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserIcon")
                        .HasColumnType("text")
                        .HasColumnName("UserIcon");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<string>("UserPassword")
                        .HasColumnType("varchar(100)")
                        .HasColumnName("UserPassword");

                    b.Property<string>("UserRole")
                        .HasColumnType("varchar(1)")
                        .HasColumnName("UserRole");

                    b.HasKey("Id");

                    b.ToTable("AspNetUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
