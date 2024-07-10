using jim.wiki.core.Auditory.Models;
using jim.wiki.core.Auditory.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jim.wiki.core.Auditory.Repository.Context.Postgress
{
    public class AuditContextPostgres : DbContext, IAuditContext
    {
        public DbSet<Audit> Audits { get; set; }
        public AuditContextPostgres()
        {

        }
        public AuditContextPostgres(DbContextOptions<AuditContextPostgres> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Common.Constantes.SCHEMA_BBDD);
            base.OnModelCreating(modelBuilder);
        }

        public async Task Add<T>(T entity)
        {
            (await AddAsync(entity)).State = EntityState.Added;


        }

        public void ApplyMigrations()
        {
            Database.Migrate();
        }
    }
}
