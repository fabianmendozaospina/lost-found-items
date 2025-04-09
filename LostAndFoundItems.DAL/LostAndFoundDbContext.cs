using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class LostAndFoundDbContext : DbContext
    {
        public LostAndFoundDbContext() { }
        public LostAndFoundDbContext(DbContextOptions<LostAndFoundDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ClaimRequest> ClaimRequests { get; set; }
        public DbSet<FoundItem> FoundItems { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LostItem> LostItems { get; set; }
        public DbSet<MatchItem> MatchItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClaimStatus> ClaimStatuses { get; set; }
        public DbSet<MatchStatus> MatchStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary Keys.
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<ClaimRequest>()
                .HasKey(cr => cr.ClaimRequestId);

            modelBuilder.Entity<ClaimStatus>()
                .HasKey(cs => cs.ClaimStatusId);

            modelBuilder.Entity<FoundItem>()
                .HasKey(fi => fi.FoundItemId);

            modelBuilder.Entity<Location>()
                .HasKey(l => l.LocationId);

            modelBuilder.Entity<LostItem>()
                .HasKey(li => li.LostItemId);

            modelBuilder.Entity<MatchItem>()
                .HasKey(mi => mi.MatchItemId);

            modelBuilder.Entity<MatchStatus>()
                .HasKey(ms => ms.MatchStatusId);

            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            // Unique constraints.
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<ClaimStatus>()
                .HasIndex(cs => cs.Name)
                .IsUnique();

            modelBuilder.Entity<Location>()
                .HasIndex(l => l.Name)
                .IsUnique();

            modelBuilder.Entity<MatchItem>()
                .HasIndex(mi => new { mi.FoundItemId, mi.LostItemId })
                .IsUnique();

            modelBuilder.Entity<MatchStatus>()
                .HasIndex(ms => ms.Name)
                .IsUnique();

            modelBuilder.Entity<ClaimRequest>()
                .HasIndex(cr => new { cr.FoundItemId, cr.ClaimingUserId })
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Required and length restrictions.
            modelBuilder.Entity<Category>()
                      .Property(c => c.Name)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<ClaimRequest>()
                      .Property(cr => cr.FoundItemId)
                      .IsRequired();

            modelBuilder.Entity<ClaimRequest>()
                      .Property(cr => cr.ClaimingUserId)
                      .IsRequired();

            modelBuilder.Entity<ClaimRequest>()
                      .Property(cr => cr.CreatedAt)
                      .IsRequired();

            modelBuilder.Entity<ClaimRequest>()
                      .Property(cr => cr.ClaimStatusId)
                      .IsRequired();

            modelBuilder.Entity<ClaimRequest>()
                      .Property(cr => cr.Message)
                      .IsRequired();

            modelBuilder.Entity<ClaimStatus>()
                      .Property(cs => cs.Name)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<FoundItem>()
                      .Property(fi => fi.UserId)
                      .IsRequired();

            modelBuilder.Entity<FoundItem>()
                      .Property(fi => fi.LocationId)
                      .IsRequired();

            modelBuilder.Entity<FoundItem>()
                      .Property(fi => fi.Title)
                      .HasMaxLength(50)
                      .IsRequired();

            modelBuilder.Entity<FoundItem>()
                      .Property(fi => fi.Description)
                      .HasMaxLength(100)
                      .IsRequired();

            modelBuilder.Entity<FoundItem>()
                      .Property(fi => fi.FoundDate)
                      .IsRequired();

            modelBuilder.Entity<FoundItem>()
                      .Property(fi => fi.CategoryId)
                      .IsRequired();

            modelBuilder.Entity<Location>()
                      .Property(l => l.Name)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<LostItem>()
                      .Property(li => li.UserId)
                      .IsRequired();

            modelBuilder.Entity<LostItem>()
                      .Property(li => li.LocationId)
                      .IsRequired();

            modelBuilder.Entity<LostItem>()
                      .Property(li => li.Title)
                      .HasMaxLength(50)
                      .IsRequired();

            modelBuilder.Entity<LostItem>()
                      .Property(li => li.Description)
                      .HasMaxLength(100)
                      .IsRequired();

            modelBuilder.Entity<LostItem>()
                      .Property(li => li.LostDate)
                      .IsRequired();

            modelBuilder.Entity<LostItem>()
                      .Property(li => li.CategoryId)
                      .IsRequired();

            modelBuilder.Entity<MatchItem>()
                      .Property(mi => mi.FoundItemId)
                      .IsRequired();

            modelBuilder.Entity<MatchItem>()
                      .Property(mi => mi.LostItemId)
                      .IsRequired();

            modelBuilder.Entity<MatchItem>()
                      .Property(mi => mi.Observation)
                      .IsRequired();

            modelBuilder.Entity<MatchItem>()
                      .Property(mi => mi.MatchUserId)
                      .IsRequired();

            modelBuilder.Entity<MatchItem>()
                      .Property(mi => mi.MatchUserId)
                      .IsRequired();

            modelBuilder.Entity<MatchStatus>()
                      .Property(ms => ms.Name)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<Role>()
                      .Property(r => r.Name)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<User>()
                      .Property(u => u.FirstName)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<User>()
                      .Property(u => u.LastName)
                      .HasMaxLength(30)
                      .IsRequired();

            modelBuilder.Entity<User>()
                      .Property(u => u.Email)
                      .HasMaxLength(60)
                      .IsRequired();

            modelBuilder.Entity<User>()
                      .Property(u => u.RoleId)
                      .IsRequired();

            // Relationships.
            modelBuilder.Entity<FoundItem>()
                .HasOne(fi => fi.Category)
                .WithMany(c => c.FoundItems)
                .HasForeignKey(fi => fi.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FoundItem>()
                .HasOne(fi => fi.Location)
                .WithMany(l => l.FoundItems)
                .HasForeignKey(li => li.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LostItem>()
                .HasOne(li => li.Category)
                .WithMany(c => c.LostItems)
                .HasForeignKey(li => li.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LostItem>()
                .HasOne(li => li.Location)
                .WithMany(l => l.LostItems)
                .HasForeignKey(li => li.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchItem>()
                .HasOne(mi => mi.FoundItem)
                .WithOne(fi => fi.MatchItem)
                .HasForeignKey<MatchItem>(mi => mi.FoundItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchItem>()
                .HasOne(mi => mi.LostItem)
                .WithOne(li => li.MatchItem)
                .HasForeignKey<MatchItem>(mi => mi.LostItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClaimRequest>()
                .HasOne(cr => cr.FoundItem)
                .WithMany(u => u.ClaimRequests)
                .HasForeignKey(cr => cr.ClaimingUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClaimRequest>()
                .HasOne(cr => cr.User)
                .WithMany(u => u.ClaimRequests)
                .HasForeignKey(cr => cr.ClaimingUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LostItem>()
                .HasOne(li => li.User)
                .WithMany(u => u.LostItems)
                .HasForeignKey(li => li.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchItem>()
                .HasOne(mi => mi.User)
                .WithMany(u => u.MatchItems)
                .HasForeignKey(mi => mi.MatchUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchItem>()
                .HasOne(mi => mi.MatchStatus)
                .WithMany(ms => ms.MatchItems)
                .HasForeignKey(mi => mi.MatchStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClaimRequest>()
                .HasOne(cr => cr.ClaimStatus)
                .WithMany(cs => cs.ClaimRequests)
                .HasForeignKey(cr => cr.ClaimStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
