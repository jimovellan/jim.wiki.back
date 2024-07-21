using jim.wiki.back.core.Repository.Abstractions;
using jim.wiki.core.Repository.Models.Search;
using jim.wiki.core.Results;
using System.Linq.Expressions;

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

        public Task<ResultSearch<T>> ApplyFilterToSearch<T>( IQueryable<T> customQuery , FilterSearch filter = null);

    public Task<ResultSearch<TOut>> ApplyFilterToSearch<T, TOut>(IQueryable<T> customQuery, FilterSearch filter, Func<T, TOut> converterFunction);


        

    }

