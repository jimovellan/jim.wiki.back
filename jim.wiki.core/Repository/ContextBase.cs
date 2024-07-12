using jim.wiki.back.core.Repository.Abstractions;
using jim.wiki.core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Repository
{
    public abstract class ContextBase:DbContext,IUnitOfWork
    {
        

        protected ContextBase()
        {
            
        }

        public ContextBase(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            OnModelCreatingCustom(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        protected virtual void OnModelCreatingCustom(ModelBuilder modelBuilder)
        {

            RegisterFilters(modelBuilder);
        }

        private void RegisterFilters(ModelBuilder modelBuilder)
        {
            RegisterFilterHideDeletedRows(modelBuilder);
        }

        private void RegisterFilterHideDeletedRows(ModelBuilder modelBuilder)
        {
            // Obtener todas las entidades que implementan IDeleted
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(ILogicalEntity).IsAssignableFrom(e.ClrType));

            foreach (var entityType in entityTypes)
            {
                // Obtener propiedad IsDeleted de la entidad
                var isDeletedProperty = entityType.FindProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
                {
                    // Configurar el filtro global para la entidad que implementa IDeleted
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.Equal(
                        Expression.Property(parameter, isDeletedProperty.PropertyInfo),
                        Expression.Constant(false, typeof(bool)));

                    var lambda = Expression.Lambda(body, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public void BeginTransaction()
        {
            this.Database.BeginTransaction();
        }

        public void Commit()
        {
            this.Database.CommitTransaction();
        }

        public void Rollback()
        {
            this.Database.RollbackTransaction();
        }

        public async Task<int> SaveAsync()
        {
            return await this.SaveChangesAsync();
        }

        public  bool HasTransaction()
        {
            return this.Database.CurrentTransaction != null;
        }
    }
}
