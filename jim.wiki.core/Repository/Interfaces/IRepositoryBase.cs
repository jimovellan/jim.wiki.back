using jim.wiki.back.core.Repository.Abstractions;

namespace jim.wiki.core.Repository.Interfaces;

    public interface IRepositoryBase<TEntity> where TEntity : Aggregate
    {

        public IQueryable<TEntity> Query();

        public Task<TEntity> UpdateAsync(TEntity entity);

        public Task DeleteAsync(TEntity entity);

        public Task<bool> AddAsync(TEntity entity);

        public Task<long> AddAndSaveAsync(TEntity entity);

        public Task<TEntity> GetById(long id);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }

