using LightNap.Core.Data.Converters;
using LightNap.Core.Data.Entities;
using LightNap.Core.Profile.Dto.Response;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Data
{
    /// <summary>
    /// Represents the application database context.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        /// <summary>
        /// Gets or sets the refresh tokens DbSet.
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        /// <summary>
        /// Gets or sets the class info DbSet.
        /// </summary>
        public DbSet<ClassInfo> ClassInfos { get; set; } = null!;

        /// <summary>
        /// Gets or sets the trade requests DbSet.
        /// </summary>
        public DbSet<TradeRequest> TradeRequests { get; set; } = null!;

        /// <summary>
        /// Gets or sets the class users DbSet.
        /// </summary>
        public DbSet<ClassUser> ClassUsers { get; set; } = null!;

        /// <summary>
        /// Gets or sets the class desires DbSet.
        /// <summary>
        public DbSet<ClassDesire> ClassDesires { get; set; } = null!;

        /// <summary>
        /// Gets or sets the class requests DbSet.
        /// </summary>
        public DbSet<ClassRequest> ClassRequests { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The DbContext options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext() { }

        ///// <inheritdoc />
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseLazyLoadingProxies();
        //    }
        //}

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (this.Database.IsSqlite())
            {
                // Allow case-insensitive queries for SQLite.

                builder.Entity<ApplicationUser>()
                    .Property(u => u.UserName)
                    .UseCollation("NOCASE");

                builder.Entity<ApplicationUser>()
                    .Property(u => u.Email)
                    .UseCollation("NOCASE");
            }

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .IsRequired();
        }

        /// <inheritdoc />
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Make sure all DateTime properties are stored as UTC.
            configurationBuilder.Properties<DateTime>().HaveConversion(typeof(UtcValueConverter));

            // Storing this as a JSON string.
            configurationBuilder.Properties<BrowserSettingsDto>().HaveConversion(typeof(BrowserSettingsConverter));
        }

        public async Task<ClassUser?> GetUserInActiveClassAsync(int classId, string userId)
        {
            return await this.ClassUsers.FirstOrDefaultAsync((classUser) => classUser.UserId == userId && classUser.ClassId == classId && classUser.IsActive);
        }

        public async Task<ClassDesire?> GetClassOnActiveUserWishlistAsync(int classId, string userId)
        {
            return await this.ClassDesires.FirstOrDefaultAsync((classDesire) => classDesire.UserId == userId && classDesire.ClassId == classId && classDesire.IsActive);
        }
    }
}
