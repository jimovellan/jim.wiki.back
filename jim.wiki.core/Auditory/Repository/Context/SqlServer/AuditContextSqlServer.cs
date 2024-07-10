using jim.wiki.core.Auditory.Models;
using jim.wiki.core.Auditory.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jim.wiki.core.Auditory.Repository.Context.SqlServer
{
    public class AuditContextSqlSever : DbContext, IAuditContext
    {
        public DbSet<Audit> Audits { get; set; }
        public AuditContextSqlSever()
        {

        }
        public AuditContextSqlSever(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer();
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
            await AddAsync(entity);
        }
        public void ApplyMigrations()
        {
            Database.Migrate();
        }
    }
}
