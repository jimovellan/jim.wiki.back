using jim.wiki.back.core.Repository.Abstractions;
using jim.wiki.core.Extensions;
using jim.wiki.core.Repository.Interfaces;
using jim.wiki.core.Repository.Models.Search;
using Microsoft.EntityFrameworkCore;
using jim.wiki.core.Repository.Extensions;
using jim.wiki.core.Results;

namespace jim.wiki.back.infrastructure.Repository
{
    public class RepositoryGeneric<TEntity> : IRepositoryBase<TEntity> where TEntity : Aggregate
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

        public async Task<ResultSearch<TEntity>> ApplyFilterToSearch(FilterSearch filter)
        {
            //validate filter
            filter.Validate();

            //validate if exists the fields into the entity
            if (filter.Filter.ContainElements())
            {    Type type = typeof(TEntity);
                foreach (var field in filter.Filter)
                {
                    if (!type.ContainsProperty(field.Name)) throw new Exception($"the field {field.Name} no exist in entity {type.Name}");
                }
            }

            var query = _dbSet.AsQueryable();

            //filter result

            query = query.Filter(filter.Filter);

            //order result
            var isFirstOrder = true;
            foreach (var order in filter.Order)
            {
                var isAscending = order?.Direction?.Trim().ToLower() == "asc";

                if (isFirstOrder)
                {
                    query = query.OrderBy(order.Field, isAscending);
                    isFirstOrder = false;
                }
                else
                {
                    query = ((IOrderedQueryable<TEntity>)query).ThenOrderBy(order.Field, isAscending);
                }
            }

            var total = await query.CountAsync();
           
            //Pagination
            if(filter.Pagination != null && (filter.Pagination.Take ?? 0) > 0 && (filter.Pagination.Page ?? 0) > 0)
            {
                query = query.Skip(((filter.Pagination?.Page ?? 0) - 1) * (filter?.Pagination?.Page ?? 0))
                            .Take((filter.Pagination?.Take ?? 0));
                            
            }

            var list = await query.ToListAsync();
            return new ResultSearch<TEntity>(list,total,filter.Pagination.Page, filter.Pagination.Take);
            
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
            if (entity is IAuditEntity logicalEntity)
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
                                            && wh.Entity.GetType().IsSubclassOf(typeof(ILogicalEntity)));

            if (logicalEntities != null)
            {
                foreach (var entity in logicalEntities)
                {
                    entity.State = EntityState.Modified;
                    ((ILogicalEntity)entity.Entity).IsDeleted = true;
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
                    if (entity.Entity is IAuditEntity audit)
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
                           
                            audit.LastAction = ((entity.Entity as ILogicalEntity)?.IsDeleted ?? false) ? "Deleted" : "Modified";

                        }

                    }
                }
            }
        }
    }
}

