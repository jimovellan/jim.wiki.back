using jim.wiki.back.model.Models.Users;
using jim.wiki.core.Repository;
using jim.wiki.core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace jim.wiki.back.infrastructure.Repository
{
    public class ApplicationContext: ContextBase
    {
        

        public ApplicationContext():base() { }

        public ApplicationContext(DbContextOptions options, IDataBaseSeeder<ModelBuilder> dataBaseSeeder):base(options, dataBaseSeeder)
        {
           
        }

        

        #region Dbsets
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql();
            }

            base.OnConfiguring(optionsBuilder);
        }

        public override void OnModelCreatingCustom(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreatingCustom(modelBuilder);
        }

        public void AplyMigrations()
        {
            this.Database.Migrate();
        }

        public DbSet<User>  Users { get; set; }
        public DbSet<Rol> Roles { get; set; }
   

        public DbSet<UserToken> UserTokens { get; set; }
        #endregion
    }
}
