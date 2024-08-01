﻿// <auto-generated />
using System;
using EventManagementApp.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EventManagementApp.Migrations
{
    [DbContext(typeof(EventManagementDBContext))]
    partial class EventManagementDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventManagementApp.Models.ClientResponse", b =>
                {
                    b.Property<int>("ClientResponseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientResponseId"));

                    b.Property<string>("ClientDecision")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ClientResponseDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("QuotationResponseId")
                        .HasColumnType("int");

                    b.HasKey("ClientResponseId");

                    b.HasIndex("QuotationResponseId")
                        .IsUnique();

                    b.ToTable("ClientResponses");
                });

            modelBuilder.Entity("EventManagementApp.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Maplink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfTickets")
                        .HasColumnType("int");

                    b.Property<string>("Poster")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RemainingTickets")
                        .HasColumnType("int");

                    b.Property<decimal>("TicketCost")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("EventId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EventManagementApp.Models.EventCategory", b =>
                {
                    b.Property<int>("EventCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventCategoryId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsService")
                        .HasColumnType("bit");

                    b.Property<int>("NumberOfRatings")
                        .HasColumnType("int");

                    b.Property<string>("Poster")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Rating")
                        .HasColumnType("real");

                    b.Property<float>("TotalRating")
                        .HasColumnType("real");

                    b.HasKey("EventCategoryId");

                    b.ToTable("EventCategories");
                });

            modelBuilder.Entity("EventManagementApp.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int>("ClientResponseId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventCategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("ClientResponseId")
                        .IsUnique();

                    b.HasIndex("EventCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("EventManagementApp.Models.QuotationRequest", b =>
                {
                    b.Property<int>("QuotationRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuotationRequestId"));

                    b.Property<string>("CateringInstructions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventCategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EventEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FoodPreference")
                        .HasColumnType("int");

                    b.Property<string>("LocationDetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuotationStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SpecialInstructions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("VenueType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuotationRequestId");

                    b.HasIndex("EventCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("QuotationRequests");
                });

            modelBuilder.Entity("EventManagementApp.Models.QuotationResponse", b =>
                {
                    b.Property<int>("QuotationResponseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuotationResponseId"));

                    b.Property<int?>("Currency")
                        .HasColumnType("int");

                    b.Property<int>("QuotationRequestId")
                        .HasColumnType("int");

                    b.Property<double?>("QuotedAmount")
                        .HasColumnType("float");

                    b.Property<string>("RequestStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ResponseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResponseMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuotationResponseId");

                    b.HasIndex("QuotationRequestId")
                        .IsUnique();

                    b.ToTable("QuotationResponses");
                });

            modelBuilder.Entity("EventManagementApp.Models.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<int>("ClientResponseId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventCategoryId")
                        .HasColumnType("int");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReviewId");

                    b.HasIndex("ClientResponseId")
                        .IsUnique();

                    b.HasIndex("EventCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("EventManagementApp.Models.ScheduledEvent", b =>
                {
                    b.Property<int>("ScheduledEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduledEventId"));

                    b.Property<int>("ClienResponseId")
                        .HasColumnType("int");

                    b.Property<int>("EventCategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("QuotationRequestId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ScheduledEventId");

                    b.HasIndex("ClienResponseId")
                        .IsUnique();

                    b.HasIndex("EventCategoryId");

                    b.HasIndex("QuotationRequestId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("ScheduledEvents");
                });

            modelBuilder.Entity("EventManagementApp.Models.Tickets", b =>
                {
                    b.Property<string>("TicketId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AttendeeEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AttendeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CheckedInTickets")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfTickets")
                        .HasColumnType("int");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TicketCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TicketId");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("EventManagementApp.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            CreatedAt = new DateTime(2024, 8, 1, 17, 14, 38, 104, DateTimeKind.Local).AddTicks(6361),
                            Email = "bookmyevent24@gmail.com",
                            FullName = "Book My Event",
                            ProfileUrl = "",
                            Role = "Admin"
                        });
                });

            modelBuilder.Entity("EventManagementApp.Models.ClientResponse", b =>
                {
                    b.HasOne("EventManagementApp.Models.QuotationResponse", "QuotationResponse")
                        .WithOne("ClientResponse")
                        .HasForeignKey("EventManagementApp.Models.ClientResponse", "QuotationResponseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("QuotationResponse");
                });

            modelBuilder.Entity("EventManagementApp.Models.Order", b =>
                {
                    b.HasOne("EventManagementApp.Models.ClientResponse", "ClientResponse")
                        .WithOne("Order")
                        .HasForeignKey("EventManagementApp.Models.Order", "ClientResponseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.EventCategory", "EventCategory")
                        .WithMany("Orders")
                        .HasForeignKey("EventCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ClientResponse");

                    b.Navigation("EventCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventManagementApp.Models.QuotationRequest", b =>
                {
                    b.HasOne("EventManagementApp.Models.EventCategory", "EventCategory")
                        .WithMany("QuotationRequests")
                        .HasForeignKey("EventCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.User", "User")
                        .WithMany("QuotationRequests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("EventCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventManagementApp.Models.QuotationResponse", b =>
                {
                    b.HasOne("EventManagementApp.Models.QuotationRequest", "QuotationRequest")
                        .WithOne("QuotationResponse")
                        .HasForeignKey("EventManagementApp.Models.QuotationResponse", "QuotationRequestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("QuotationRequest");
                });

            modelBuilder.Entity("EventManagementApp.Models.Review", b =>
                {
                    b.HasOne("EventManagementApp.Models.ClientResponse", "ClientResponse")
                        .WithOne("Review")
                        .HasForeignKey("EventManagementApp.Models.Review", "ClientResponseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.EventCategory", "EventCategory")
                        .WithMany("Reviews")
                        .HasForeignKey("EventCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ClientResponse");

                    b.Navigation("EventCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventManagementApp.Models.ScheduledEvent", b =>
                {
                    b.HasOne("EventManagementApp.Models.ClientResponse", "ClientResponse")
                        .WithOne()
                        .HasForeignKey("EventManagementApp.Models.ScheduledEvent", "ClienResponseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.EventCategory", "EventCategory")
                        .WithMany("ScheduledEvents")
                        .HasForeignKey("EventCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.QuotationRequest", "QuotationRequest")
                        .WithOne()
                        .HasForeignKey("EventManagementApp.Models.ScheduledEvent", "QuotationRequestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.User", "User")
                        .WithMany("ScheduledEvents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ClientResponse");

                    b.Navigation("EventCategory");

                    b.Navigation("QuotationRequest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventManagementApp.Models.Tickets", b =>
                {
                    b.HasOne("EventManagementApp.Models.Event", "Event")
                        .WithMany("Tickets")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventManagementApp.Models.User", "User")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EventManagementApp.Models.ClientResponse", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();

                    b.Navigation("Review")
                        .IsRequired();
                });

            modelBuilder.Entity("EventManagementApp.Models.Event", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("EventManagementApp.Models.EventCategory", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("QuotationRequests");

                    b.Navigation("Reviews");

                    b.Navigation("ScheduledEvents");
                });

            modelBuilder.Entity("EventManagementApp.Models.QuotationRequest", b =>
                {
                    b.Navigation("QuotationResponse")
                        .IsRequired();
                });

            modelBuilder.Entity("EventManagementApp.Models.QuotationResponse", b =>
                {
                    b.Navigation("ClientResponse")
                        .IsRequired();
                });

            modelBuilder.Entity("EventManagementApp.Models.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("QuotationRequests");

                    b.Navigation("Reviews");

                    b.Navigation("ScheduledEvents");

                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
