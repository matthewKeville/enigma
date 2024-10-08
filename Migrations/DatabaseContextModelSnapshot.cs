﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace crossword.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Entity.Crossword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Columns")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("Elapsed")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("FinishDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Published")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rows")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WordCheckCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Crosswords");
                });

            modelBuilder.Entity("Entity.GridChar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<char>("C")
                        .HasColumnType("TEXT");

                    b.Property<int>("CrosswordId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CrosswordId");

                    b.ToTable("GridChars");
                });

            modelBuilder.Entity("Entity.Word", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Clue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CrosswordId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Direction")
                        .HasColumnType("INTEGER");

                    b.Property<int>("I")
                        .HasColumnType("INTEGER");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CrosswordId");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("Entity.GridChar", b =>
                {
                    b.HasOne("Entity.Crossword", "crossword")
                        .WithMany("GridChars")
                        .HasForeignKey("CrosswordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("crossword");
                });

            modelBuilder.Entity("Entity.Word", b =>
                {
                    b.HasOne("Entity.Crossword", "crossword")
                        .WithMany("Words")
                        .HasForeignKey("CrosswordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("crossword");
                });

            modelBuilder.Entity("Entity.Crossword", b =>
                {
                    b.Navigation("GridChars");

                    b.Navigation("Words");
                });
#pragma warning restore 612, 618
        }
    }
}
