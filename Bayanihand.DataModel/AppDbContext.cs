using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayanihand.DataModel
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Almirol
            optionsBuilder.UseSqlServer("server=GYALAPTOP\\SQLEXPRESS; " +
                "database=almirol_entprog; uid=eisensy_student; " +
                "pwd=Benilde@2020; integrated security=sspi; " +
                "trustservercertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(r => new { r.UserId, r.RoleId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            // Foreign Key relationship between Cart and CartCommission
            modelBuilder.Entity<CartCommission>()
                .HasOne(cc => cc.Cart)
                .WithMany(c => c.Items) 
                .HasForeignKey(cc => cc.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign Key between CartCommission and Service
            modelBuilder.Entity<CartCommission>()
                .HasOne(cc => cc.Service)
                .WithMany()
                .HasForeignKey(cc => cc.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign Key between CartCommission and Schedule
            modelBuilder.Entity<CartCommission>()
                .HasOne(cc => cc.Schedule)
                .WithMany()
                .HasForeignKey(cc => cc.ScheduleId)
                .OnDelete(DeleteBehavior.SetNull);

            // Foreign Key between CartCommission and Rating
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.CartCommission)
                .WithMany(c => c.Ratings)
                .HasForeignKey(r => r.CartCommissionId)
                .OnDelete(DeleteBehavior.Cascade);



        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartCommission> CartCommissions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Handyman> Handymen { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
