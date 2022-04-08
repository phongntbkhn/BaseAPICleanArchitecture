using EntityModel.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext(IConfiguration configuration)
           : base(GetOptions(configuration))
        {
        }

        private static DbContextOptions GetOptions(IConfiguration configuration)
        {
            var envTemp = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var env = string.Empty;
            if (envTemp != null && envTemp.ToLower() == "uat")
                env = envTemp.ToLower();
            else if (envTemp != null && envTemp.ToLower() == "development")
                env = "dev";
            string conn = ConnectionString(configuration);
            return NpgsqlDbContextOptionsBuilderExtensions.UseNpgsql(new DbContextOptionsBuilder(), conn).Options;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Override default AspNet Identity table names
            builder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<ApplicationRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<Guid>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<Guid>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<Guid>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<Guid>>(entity => { entity.ToTable("UserTokens"); });
            builder.Entity<IdentityRoleClaim<Guid>>(entity => { entity.ToTable("RoleClaims"); });
        }

        private static string ConnectionString(IConfiguration configuration)
        {
            var conn = Environment.GetEnvironmentVariable("CONNECTION");
            return conn;
        }
    }
}
