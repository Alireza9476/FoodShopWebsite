using FoodShopWebsite.Domain.Model;
using System.Linq.Expressions;

namespace FoodShopWebsite.Repository
{
    public interface IRepositoryBase<TEntity>
    {
        IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? sortOrder = null,
            string? include = null
            );

        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string ? include = null);

        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string? include = null
            );

        TEntity? GetSingle(Expression<Func<TEntity, bool>>? filter = null, string? include = null);

        TEntity Create(TEntity newModel);

        bool Edit(TEntity newModel); //Eigene Änderung, weil kein Exception

        bool Delete(TEntity model);
    }
}
