using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TourNest.Models.BloggingPlatform;
using TourNest.Models.Destinations;
using TourNest.Models.Destinations.Attractions_Details;
using TourNest.Models.Emergencies;
using TourNest.Models.Flight_Bookings;
using TourNest.Models.Payment_Details;
using TourNest.Models.Rating_and_Reviews;
using TourNest.Models.TravelExpenses.PreDefined_Budgets;
using TourNest.Models.TravelExpenses.UserBudget;
using TourNest.Models.UserProfile;

namespace TourNest.Models;

public partial class TourNestContext : DbContext
{
    public TourNestContext(DbContextOptions<TourNestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blogging> Bloggings { get; set; }

    public virtual DbSet<BookingDetail> BookingDetails { get; set; }

    public virtual DbSet<DomesticAttraction> DomesticAttractions { get; set; }

    public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; }

    public virtual DbSet<InternationalAttraction> InternationalAttractions { get; set; }

    public virtual DbSet<LuxuryBudget> LuxuryBudgets { get; set; }

    public virtual DbSet<MidRangeBudget> MidRangeBudgets { get; set; }

    public virtual DbSet<NormalBudget> NormalBudgets { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<TopDestinationsInIndium> TopDestinationsInIndia { get; set; }

    public virtual DbSet<TopPlace> TopPlaces { get; set; }

    public virtual DbSet<TopTemple> TopTemples { get; set; }

    public virtual DbSet<TravellerDetail> TravellerDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserExpense> UserExpenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blogging>(entity =>
        {
            entity.HasKey(e => e.BlogId);

            entity.ToTable("Blogging");

            entity.Property(e => e.BlogId)
                .ValueGeneratedNever()
                .HasColumnName("Blog_Id");
            entity.Property(e => e.Caption).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(1000);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Bloggings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blogging_User");
        });

        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.HasKey(e => e.BookingId);

            entity.Property(e => e.BookingId)
                .ValueGeneratedNever()
                .HasColumnName("Booking_Id");
            entity.Property(e => e.ArrivalAirport)
                .HasMaxLength(100)
                .HasColumnName("Arrival_Airport");
            entity.Property(e => e.CabinClass)
                .HasMaxLength(100)
                .HasColumnName("Cabin_Class");
            entity.Property(e => e.DepartureAirport)
                .HasMaxLength(100)
                .HasColumnName("Departure_Airport");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PlaneNumber)
                .HasMaxLength(50)
                .HasColumnName("Plane_Number");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TravellerDate)
                .HasColumnType("datetime")
                .HasColumnName("Traveller_Date");

            entity.HasOne(d => d.User).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingDetails_User");
        });

        modelBuilder.Entity<DomesticAttraction>(entity =>
        {
            entity.ToTable("Domestic_Attraction");

            entity.Property(e => e.Attraction1).HasMaxLength(1000);
            entity.Property(e => e.Attraction2).HasMaxLength(1000);
            entity.Property(e => e.Attraction3).HasMaxLength(1000);
            entity.Property(e => e.Attraction4).HasMaxLength(1000);
            entity.Property(e => e.Attraction5).HasMaxLength(1000);
            entity.Property(e => e.AttractionHeading)
                .HasMaxLength(50)
                .HasColumnName("attraction_heading");
            entity.Property(e => e.Destination).HasMaxLength(1000);
            entity.Property(e => e.Heading1).HasMaxLength(1000);
            entity.Property(e => e.Heading2).HasMaxLength(1000);
            entity.Property(e => e.Heading3).HasMaxLength(1000);
            entity.Property(e => e.Heading4).HasMaxLength(1000);
            entity.Property(e => e.Heading5).HasMaxLength(1000);
            entity.Property(e => e.Paragraph1).HasMaxLength(1000);
            entity.Property(e => e.Paragraph2).HasMaxLength(1000);
            entity.Property(e => e.Paragraph3).HasMaxLength(1000);
            entity.Property(e => e.Paragraph4).HasMaxLength(1000);
            entity.Property(e => e.Paragraph5).HasMaxLength(1000);
        });

        modelBuilder.Entity<EmergencyContact>(entity =>
        {
            entity.ToTable("EmergencyContact");

            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.EmbassyAddress)
                .HasMaxLength(500)
                .HasColumnName("Embassy_Address");
            entity.Property(e => e.EmbassyEmail)
                .HasMaxLength(100)
                .HasColumnName("Embassy_Email");
            entity.Property(e => e.EmbassyPhone)
                .HasMaxLength(50)
                .HasColumnName("Embassy_Phone");
            entity.Property(e => e.Location).HasMaxLength(50);
        });

        modelBuilder.Entity<InternationalAttraction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("InternationalAttraction");

            entity.Property(e => e.Attraction1).HasMaxLength(1000);
            entity.Property(e => e.Attraction2).HasMaxLength(1000);
            entity.Property(e => e.Attraction3).HasMaxLength(1000);
            entity.Property(e => e.Attraction4).HasMaxLength(1000);
            entity.Property(e => e.Attraction5).HasMaxLength(1000);
            entity.Property(e => e.AttractionHeading)
                .HasMaxLength(50)
                .HasColumnName("attraction_heading");
            entity.Property(e => e.Destination).HasMaxLength(1000);
            entity.Property(e => e.Heading1).HasMaxLength(1000);
            entity.Property(e => e.Heading2).HasMaxLength(1000);
            entity.Property(e => e.Heading3).HasMaxLength(1000);
            entity.Property(e => e.Heading4).HasMaxLength(1000);
            entity.Property(e => e.Heading5).HasMaxLength(1000);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Paragraph1).HasMaxLength(1000);
            entity.Property(e => e.Paragraph2).HasMaxLength(1000);
            entity.Property(e => e.Paragraph3).HasMaxLength(1000);
            entity.Property(e => e.Paragraph4).HasMaxLength(1000);
            entity.Property(e => e.Paragraph5).HasMaxLength(1000);
        });

        modelBuilder.Entity<LuxuryBudget>(entity =>
        {
            entity.ToTable("LuxuryBudget");

            entity.Property(e => e.Accommodation)
                .HasMaxLength(100)
                .HasColumnName("accommodation");
            entity.Property(e => e.Attractions)
                .HasMaxLength(100)
                .HasColumnName("attractions");
            entity.Property(e => e.Food)
                .HasMaxLength(100)
                .HasColumnName("food");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Miscellaneous)
                .HasMaxLength(100)
                .HasColumnName("miscellaneous");
            entity.Property(e => e.Total).HasMaxLength(100);
            entity.Property(e => e.Transportation)
                .HasMaxLength(100)
                .HasColumnName("transportation");
        });

        modelBuilder.Entity<MidRangeBudget>(entity =>
        {
            entity.ToTable("MidRangeBudget");

            entity.Property(e => e.Accommodation)
                .HasMaxLength(100)
                .HasColumnName("accommodation");
            entity.Property(e => e.Attractions)
                .HasMaxLength(100)
                .HasColumnName("attractions");
            entity.Property(e => e.Food)
                .HasMaxLength(100)
                .HasColumnName("food");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Miscellaneous)
                .HasMaxLength(100)
                .HasColumnName("miscellaneous");
            entity.Property(e => e.Total).HasMaxLength(100);
            entity.Property(e => e.Transportation)
                .HasMaxLength(100)
                .HasColumnName("transportation");
        });

        modelBuilder.Entity<NormalBudget>(entity =>
        {
            entity.ToTable("NormalBudget");

            entity.Property(e => e.Accommodation)
                .HasMaxLength(100)
                .HasColumnName("accommodation");
            entity.Property(e => e.Attractions)
                .HasMaxLength(100)
                .HasColumnName("attractions");
            entity.Property(e => e.Food)
                .HasMaxLength(100)
                .HasColumnName("food");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Miscellaneous)
                .HasMaxLength(100)
                .HasColumnName("miscellaneous");
            entity.Property(e => e.Total).HasMaxLength(100);
            entity.Property(e => e.Transportation)
                .HasMaxLength(100)
                .HasColumnName("transportation");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("Payment");

            entity.Property(e => e.OrderId)
                .HasMaxLength(70)
                .HasColumnName("Order_id");
            entity.Property(e => e.Amount).HasMaxLength(10);
            entity.Property(e => e.BookingId).HasColumnName("booking_Id");
            entity.Property(e => e.PaymentId)
                .HasMaxLength(150)
                .HasColumnName("Payment_id");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(10)
                .HasColumnName("Payment Status");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_BookingDetails");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_User");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");

            entity.Property(e => e.ReviewId)
                .ValueGeneratedNever()
                .HasColumnName("Review_id");
            entity.Property(e => e.CreatedAt).HasColumnName("Created At");
            entity.Property(e => e.Review1)
                .HasMaxLength(100)
                .HasColumnName("Review");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_User");
        });

        modelBuilder.Entity<TopDestinationsInIndium>(entity =>
        {
            entity.HasKey(e => e.IndiaDestinationId);

            entity.ToTable("Top_Destinations_in_India");

            entity.Property(e => e.IndiaDestinationId).HasColumnName("India_Destination_Id");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Photo).HasMaxLength(500);
        });

        modelBuilder.Entity<TopPlace>(entity =>
        {
            entity.ToTable("Top_Places");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Photo)
                .HasMaxLength(200)
                .HasColumnName("photo");
        });

        modelBuilder.Entity<TopTemple>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Photo).HasMaxLength(250);
        });

        modelBuilder.Entity<TravellerDetail>(entity =>
        {
            entity.HasKey(e => e.TravellerId);

            entity.ToTable("Traveller Detail");

            entity.Property(e => e.TravellerId)
                .ValueGeneratedNever()
                .HasColumnName("Traveller_Id");
            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Booking).WithMany(p => p.TravellerDetails)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Traveller Detail_BookingDetails");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(60);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.ProfilePhoto).HasMaxLength(1000);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<UserExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId);

            entity.ToTable("UserExpense");

            entity.Property(e => e.ExpenseId)
                .ValueGeneratedNever()
                .HasColumnName("Expense_Id");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Notes).HasMaxLength(100);
            entity.Property(e => e.TripName).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.UserExpenses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserExpense_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
