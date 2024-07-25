using System.Security.Cryptography;
using System.Text;
using EventManagementApp.Enums;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Context
{
    public class EventManagementDBContext : DbContext
    {
        public EventManagementDBContext(DbContextOptions options) : base(options) { }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        #region DbSets
        public DbSet<ClientResponse> ClientResponses { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<QuotationRequest> QuotationRequests { get; set; }
        public DbSet<QuotationResponse> QuotationResponses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
   
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ScheduledEvent> ScheduledEvents { get; set; }

        public DbSet<User> Users { get; set; }

        #endregion

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Review>().Where(e => e.State == EntityState.Added))
            {
                var review = entry.Entity;

                var eventCategory = EventCategories.Find(review.EventCategoryId);

                if (eventCategory != null)
                {

                    float totalRating = eventCategory.TotalRating;
                    int numberOfReviews = eventCategory.NumberOfRatings;

                    totalRating += review.Rating;
                    numberOfReviews++;

                    float newRating = (float)totalRating / numberOfReviews;

                    eventCategory.Rating = newRating;

                    eventCategory.TotalRating = totalRating;
                    eventCategory.NumberOfRatings = numberOfReviews;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
               .HasMany(e => e.Tickets)
               .WithOne(t => t.Event)
               .HasForeignKey(t => t.EventId);


            modelBuilder.Entity<User>()
                .HasMany(u => u.Tickets)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Event>()
       .Property(e => e.TicketCost)
       .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<Tickets>()
                .Property(t => t.TicketCost)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<ClientResponse>()
                .HasOne(c => c.QuotationResponse)
                .WithOne(q => q.ClientResponse)
                .HasForeignKey<ClientResponse>(c => c.QuotationResponseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.EventCategory)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EventCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ClientResponse)
                .WithOne(c => c.Order)
                .HasForeignKey<Order>(o => o.ClientResponseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuotationRequest>()
                .HasOne(o => o.User)
                .WithMany(u => u.QuotationRequests)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuotationRequest>()
                .HasOne(o => o.EventCategory)
                .WithMany(e => e.QuotationRequests)
                .HasForeignKey(o => o.EventCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuotationResponse>()
                .HasOne(q => q.QuotationRequest)
                .WithOne(q => q.QuotationResponse)
                .HasForeignKey<QuotationResponse>(q => q.QuotationRequestId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Review>()
                .HasOne(r => r.ClientResponse)
                .WithOne(c => c.Review)
                .HasForeignKey<Review>(r => r.ClientResponseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r=>r.EventCategory)
                .WithMany(e=>e.Reviews)
                .HasForeignKey(r=>r.EventCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u=>u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScheduledEvent>()
                .HasOne(s=>s.EventCategory)
                .WithMany(e=>e.ScheduledEvents)
                .HasForeignKey(s=>s.EventCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScheduledEvent>()
                .HasOne(s => s.ClientResponse)
                .WithOne()
                .HasForeignKey<ScheduledEvent>(s=>s.ClienResponseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScheduledEvent>()
                .HasOne(s => s.User)
                .WithMany(u => u.ScheduledEvents)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScheduledEvent>()
                .HasOne(s => s.QuotationRequest)
                .WithOne()
                .HasForeignKey<ScheduledEvent>(s => s.QuotationRequestId)
                .OnDelete(DeleteBehavior.Restrict);



  

           foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var enumProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType.IsEnum);

                foreach (var property in enumProperties)
                {
                    modelBuilder.Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion<string>();
                }
            }


            modelBuilder.Entity<User>().HasData(CreateAdminUser());


            base.OnModelCreating(modelBuilder);
        }

        private User CreateAdminUser()
        {
            User admin = new User();

            admin.Role = UserType.Admin;

            admin.UserId = 1;
            admin.FullName = "Book My Event";
            admin.Email = "bookmyevent24@gmail.com";
            return admin;
        }

    }
}
