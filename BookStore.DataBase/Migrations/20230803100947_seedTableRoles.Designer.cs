﻿// <auto-generated />
using System;
using Minotaur.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Minotaur.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230803100947_seedTableRoles")]
    partial class seedTableRoles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bookstore.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookId");

                    b.ToTable("BooksTable");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            Author = "Жауме Кабре",
                            Category = 1,
                            Description = "Человек просыпается неизвестно где - возможно, в больничной палате, но это неточно - и не помнит о себе вообще ничего. \"Зовите меня Измаил\", - предлагает он врачам, которых, за неимением других версий, нарекает Юрием Живаго и мадам Бовари.",
                            ISBN = "978-5-389-22890-0",
                            Price = "417",
                            Title = "И нас пожирает пламя"
                        });
                });

            modelBuilder.Entity("Bookstore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Художественная литература"
                        });
                });

            

            modelBuilder.Entity("Bookstore.Models.Models.Roles", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            RoleName = "Admin",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("Bookstore.Models.Models.ShoppingBasket", b =>
                {
                    b.Property<int>("BasketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BasketId"));

                    b.Property<int>("CountProduct")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BasketId");

                    b.ToTable("ShoppingBasket");

                    b.HasData(
                        new
                        {
                            BasketId = 1,
                            CountProduct = 1,
                            ProductId = 1,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("Bookstore.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ctreet")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateofBirth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HouseNumber")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            City = "Surgut",
                            Ctreet = "Lenina",
                            DateofBirth = "2018-10-25",
                            Email = "ninileo55555@gmail.com",
                            FirstName = "Nick",
                            HouseNumber = 30,
                            LastName = "Zel",
                            Password = "admin",
                            PhoneNumber = "89226578108",
                            Region = "KMAO"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
