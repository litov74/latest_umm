using API.Database.Entities;
using API.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Data
{
    public class APIContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public APIContext(DbContextOptions<APIContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<CompanyEntity> CompanyEntity { get; set; }
        public DbSet<UserEntity> UserEntity { get; set; }
        public DbSet<CompanyUserMappingEntity> CompanyUserMappingEntity { get; set; }
        public DbSet<SubscriptionTierEntity> SubscriptionTierEntities { get; set; }
        public DbSet<PlanSubscribeEntity> PlanSubscribeEntities { get; set; }
        public DbSet<CompanyHierarchyItem> CompanyHierarchyItemsEntities { get; set; }
        public DbSet<CompanyLoadingLevelEntity> CompanyLoadingLevelEntities { get; set; }
        public DbSet<CompanySettingsEntity> CompanySettingsEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyEntity>().ToTable("Companies");
            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<CompanyUserMappingEntity>().ToTable("CompanyUserMapping")
                .HasKey(x => new { x.CompanyId, x.UserId });
            modelBuilder.Entity<SubscriptionTierEntity>().ToTable("SubscriptionTierEntity");
            modelBuilder.Entity<PlanSubscribeEntity>().ToTable("PlanSubscribeEntity")
                .HasKey(x => new { x.UserId, x.SubscriptionTierId });
            modelBuilder.Entity<CompanySettingsEntity>().ToTable("CompanySettingEntity")
              .HasKey(x => new { x.CompanyId });
            modelBuilder.Seed();
        }
        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseTrackingEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var email = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseTrackingEntity)entry.Entity).CreatedAt = DateTime.UtcNow;

                    if (email != null && email.Length > 0)
                    {
                        ((BaseTrackingEntity)entry.Entity).CreatedBy = email;
                    }
                }
                ((BaseTrackingEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;

                if (email != null && email.Length > 0)
                {
                    ((BaseTrackingEntity)entry.Entity).UpdatedBy = email;
                }

            }
        }

    }
}
