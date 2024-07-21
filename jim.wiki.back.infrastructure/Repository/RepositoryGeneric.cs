using jim.wiki.back.core.Repository.Abstractions;
using jim.wiki.core.Extensions;
using jim.wiki.core.Repository.Interfaces;
using jim.wiki.core.Repository.Models.Search;
using Microsoft.EntityFrameworkCore;
using jim.wiki.core.Repository.Extensions;
using jim.wiki.core.Results;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;

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

        

        public async Task<ResultSearch<T>> ApplyFilterToSearch<T>(IQueryable<T> query, FilterSearch filter = null)
        {
            var result = await AplyFilterToSearchInternal(query, filter);

          
            return new ResultSearch<T>(result.lista, result.total, filter.Pagination.Page, filter.Pagination.Take);

        }

        public async Task<ResultSearch<TOut>> ApplyFilterToSearch<T, TOut>(IQueryable<T> customQuery, FilterSearch filter, Func<T, TOut> converterFunction)
        {

            var result = await AplyFilterToSearchInternal(customQuery, filter);

            var lista = result.lista.Select(converterFunction);

            return new ResultSearch<TOut>(lista, result.total, filter.Pagination.Page, filter.Pagination.Take);
        }

        public async Task<(int total, IEnumerable<T> lista)> AplyFilterToSearchInternal<T>(IQueryable<T> query, FilterSearch filter)
        {
            //validate filter
            filter.Validate();

            //validate if exists the fields into the entity
            if (filter.Filter.ContainElements())
            {
                Type type = typeof(T);
                foreach (var field in filter.Filter)
                {
                    if (!type.ContainsProperty(field.Name)) throw new Exception($"the field {field.Name} no exist in entity {type.Name}");
                }
            }



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
                    query = ((IOrderedQueryable<T>)query).ThenOrderBy(order.Field, isAscending);
                }
            }

            var total = await query.CountAsync();

            //Pagination
            if (filter.Pagination != null && (filter.Pagination.Take ?? 0) > 0 && (filter.Pagination.Page ?? 0) > 0)
            {
                query = query.Skip(((filter.Pagination?.Page ?? 0) - 1) * (filter?.Pagination?.Page ?? 0))
                .Take((filter.Pagination?.Take ?? 0));

            }

            var lista = await query.ToListAsync();

            return  ( total, lista);
           
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

