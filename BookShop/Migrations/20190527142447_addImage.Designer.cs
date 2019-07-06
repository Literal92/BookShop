﻿// <auto-generated />
using System;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookShop.Migrations
{
    [DbContext(typeof(BookShopContext))]
    [Migration("20190527142447_addImage")]
    partial class addImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookShop.Models.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Caption")
                        .IsRequired();

                    b.Property<string>("File");

                    b.Property<int>("LanguageID");

                    b.Property<int>("PublishYear");

                    b.Property<int>("PublisherID");

                    b.HasKey("BookID");

                    b.HasIndex("LanguageID");

                    b.HasIndex("PublisherID");

                    b.ToTable("Photo");
                });

            modelBuilder.Entity("BookShop.Models.Book_Category", b =>
                {
                    b.Property<int>("BookID");

                    b.Property<int>("CategoryID");

                    b.HasKey("BookID", "CategoryID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Book_Categories");
                });

            modelBuilder.Entity("BookShop.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName");

                    b.Property<bool?>("Delete")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("Detailes");

                    b.Property<string>("File");

                    b.Property<byte[]>("Image")
                        .HasColumnType("image");

                    b.Property<bool?>("IsPublish");

                    b.Property<int?>("ParentCategoryID");

                    b.Property<DateTime?>("PublishDate");

                    b.HasKey("CategoryID");

                    b.HasIndex("ParentCategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BookShop.Models.ContactMe", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreate");

                    b.Property<string>("Email");

                    b.Property<string>("Message");

                    b.Property<string>("Name");

                    b.HasKey("ContactId");

                    b.ToTable("ContactMes");
                });

            modelBuilder.Entity("BookShop.Models.Language", b =>
                {
                    b.Property<int>("LanguageID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LanguageName");

                    b.HasKey("LanguageID");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("BookShop.Models.Publisher", b =>
                {
                    b.Property<int>("PublisherID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PublisherName");

                    b.HasKey("PublisherID");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("BookShop.Models.Book", b =>
                {
                    b.HasOne("BookShop.Models.Language", "Language")
                        .WithMany("Books")
                        .HasForeignKey("LanguageID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookShop.Models.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookShop.Models.Book_Category", b =>
                {
                    b.HasOne("BookShop.Models.Book", "Book")
                        .WithMany("book_Categories")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BookShop.Models.Category", "Category")
                        .WithMany("book_Categories")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BookShop.Models.Category", b =>
                {
                    b.HasOne("BookShop.Models.Category", "category")
                        .WithMany("categories")
                        .HasForeignKey("ParentCategoryID");
                });
#pragma warning restore 612, 618
        }
    }
}
