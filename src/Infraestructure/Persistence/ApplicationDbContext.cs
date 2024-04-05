using ApplicationCore.Interfaces;
using Domain.Entities;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUserService currentUser)
            : base(currentTenant, options, currentUser)
        {

        }


        public DbSet<Usuarios> Usuarios => Set<Usuarios>();

        public DbSet<logs> logs => Set<logs>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
