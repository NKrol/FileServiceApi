﻿// <auto-generated />
using System;
using FileServiceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FileServiceApi.Migrations
{
    [DbContext(typeof(UserServiceDbContext))]
    partial class UserServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FileServiceApi.Entities.FilePath", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FilePaths");
                });

            modelBuilder.Entity("FileServiceApi.Entities.Files", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("FileServiceApi.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FilePathId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FilePathId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FileServiceApi.Entities.Files", b =>
                {
                    b.HasOne("FileServiceApi.Entities.User", null)
                        .WithMany("FilesSet")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("FileServiceApi.Entities.User", b =>
                {
                    b.HasOne("FileServiceApi.Entities.FilePath", "FilePath")
                        .WithMany()
                        .HasForeignKey("FilePathId");

                    b.Navigation("FilePath");
                });

            modelBuilder.Entity("FileServiceApi.Entities.User", b =>
                {
                    b.Navigation("FilesSet");
                });
#pragma warning restore 612, 618
        }
    }
}