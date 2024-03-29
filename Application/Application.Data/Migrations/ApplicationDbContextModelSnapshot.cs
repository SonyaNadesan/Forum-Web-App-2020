﻿// <auto-generated />
using System;
using Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameInUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("HasBeenViewedByParentPostOwner")
                        .HasColumnType("bit");

                    b.Property<bool>("HasBeenViewedByThreadOwner")
                        .HasColumnType("bit");

                    b.Property<bool>("HasParentPost")
                        .HasColumnType("bit");

                    b.Property<int>("LevelInHierarchy")
                        .HasColumnType("int");

                    b.Property<Guid?>("ParentPostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ThreadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ParentPostId");

                    b.HasIndex("ThreadId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Reaction", b =>
                {
                    b.Property<Guid>("ReactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("HasBeenViewedByThreadOwner")
                        .HasColumnType("bit");

                    b.Property<string>("ReactionType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ThreadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReactionId");

                    b.HasIndex("ThreadId");

                    b.HasIndex("UserId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Thread", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Heading")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TopicId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.HasIndex("UserId");

                    b.ToTable("Threads");
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameInUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureImageSrc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CategoryThread", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ThreadsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoriesId", "ThreadsId");

                    b.HasIndex("ThreadsId");

                    b.ToTable("CategoryThread");
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Post", b =>
                {
                    b.HasOne("Application.Domain.ApplicationEntities.Post", "ParentPost")
                        .WithMany()
                        .HasForeignKey("ParentPostId");

                    b.HasOne("Application.Domain.ApplicationEntities.Thread", "Thread")
                        .WithMany("Posts")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Domain.ApplicationEntities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentPost");

                    b.Navigation("Thread");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Reaction", b =>
                {
                    b.HasOne("Application.Domain.ApplicationEntities.Thread", "Thread")
                        .WithMany("Reactions")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Domain.ApplicationEntities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Thread");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Thread", b =>
                {
                    b.HasOne("Application.Domain.ApplicationEntities.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId");

                    b.HasOne("Application.Domain.ApplicationEntities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Topic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CategoryThread", b =>
                {
                    b.HasOne("Application.Domain.ApplicationEntities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Application.Domain.ApplicationEntities.Thread", null)
                        .WithMany()
                        .HasForeignKey("ThreadsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Application.Domain.ApplicationEntities.Thread", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Reactions");
                });
#pragma warning restore 612, 618
        }
    }
}
