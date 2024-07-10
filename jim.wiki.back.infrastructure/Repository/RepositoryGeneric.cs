using jim.wiki.back.core.Repository.Abstractions;
using jim.wiki.core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jim.wiki.back.infrastructure.Repository
{
    public class RepositoryGeneric<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
    {
        private readonly ApplicationContext _ctx;
        private readonly DbSet<TEntity> _dbSet;
        
        

        public RepositoryGeneric(ApplicationContext ctx)
        {
            _ctx = ctx;
            _dbSet = ctx.Set<TEntity>();
            
            
        }

        public async Task<long> AddAndSaveAsync(TEntity entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            var result = await SaveChangesAsync();
            if (result > 0)
            {
                return entry.Entity.Id;
            }

            return 0;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            var entryEntity = await _dbSet.AddAsync(entity);

            return entryEntity.State == EntityState.Added;
        }

        public Task DeleteAsync(TEntity entity)
        {

            _ctx.Set<TEntity>().Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

            return Task.CompletedTask;

        }

        public async Task<TEntity> GetById(long id)
        {
            return await _ctx.FindAsync<TEntity>(id);
        }

        public IQueryable<TEntity> Query()
        {
            return _ctx.Set<TEntity>().IgnoreQueryFilters();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ModifyLogicalEntites();
            SetAuditProperties();
            return await _ctx.SaveChangesAsync(cancellationToken);
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity is AuditEntity logicalEntity)
            {

                logicalEntity.ModifiedAt = DateTime.UtcNow;
                logicalEntity.LastAction = "Modified";
                _ctx.Set<TEntity>().Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                _ctx.Set<TEntity>().Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }

            return Task.FromResult(entity);
        }

        private void ModifyLogicalEntites()
        {
            var logicalEntities = _ctx.ChangeTracker
                                      .Entries()
                                      .Where(wh => wh.State == EntityState.Deleted
                                            && wh.Entity.GetType().IsSubclassOf(typeof(LogicalEntity)));

            if (logicalEntities != null)
            {
                foreach (var entity in logicalEntities)
                {
                    entity.State = EntityState.Modified;
                    ((LogicalEntity)entity.Entity).IsDeleted = true;
                }
            }
        }
        private void SetAuditProperties()
        {
            if (_ctx.ChangeTracker.HasChanges())
            {
                var modifiedEntities = _ctx.ChangeTracker.Entries().Where(wh => wh.State == EntityState.Modified || wh.State == EntityState.Added);

                foreach (var entity in modifiedEntities)
                {
                    if (entity.Entity is AuditEntity audit)
                    {
                        audit.ModifiedAt = DateTime.UtcNow;
                        audit.ModifiedBy = "System";
                        if (entity.State == EntityState.Added)
                        {
                            audit.CreatedBy = "System";
                            audit.CreateadAt = audit.ModifiedAt;
                            audit.LastAction = "Added";
                        }
                        else
                        {
                           
                            audit.LastAction = ((entity.Entity as LogicalEntity)?.IsDeleted ?? false) ? "Deleted" : "Modified";

                        }

                    }
                }
            }
        }
    }
}

