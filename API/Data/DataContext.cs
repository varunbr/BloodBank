using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BloodGroup> BloodGroups { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>()
                .HasOne(a => a.Bank)
                .WithOne(b => b.Address)
                .HasForeignKey<Address>(a => a.BankId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Address>()
                .HasOne(a => a.User)
                .WithOne(u => u.Address)
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppRole>()
                .HasMany(ar => ar.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            builder.Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<Moderator>()
                .HasKey(k => new { k.UserId, k.BankId });
            builder.Entity<Moderator>()
                .HasOne(m => m.User)
                .WithMany(u => u.Moderates)
                .HasForeignKey(m => m.UserId);
            builder.Entity<Moderator>()
                .HasOne(m => m.Bank)
                .WithMany(b => b.Moderators)
                .HasForeignKey(m => m.BankId);

            builder.Entity<Photo>()
                .HasOne(p => p.User)
                .WithOne(u => u.Photo)
                .HasForeignKey<Photo>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Photo>()
                .HasOne(p => p.Bank)
                .WithOne(b => b.Photo)
                .HasForeignKey<Photo>(p => p.BankId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BloodGroup>()
                .HasOne(bg => bg.Bank)
                .WithOne(b => b.BloodGroup)
                .HasForeignKey<BloodGroup>(bg => bg.BankId);
        }
    }
}
