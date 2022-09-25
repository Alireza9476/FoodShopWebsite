using System.Linq.Expressions;
using FoodShopWebsite.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FoodShopWebsite.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
    {
        private readonly TestFoodShopWebsiteDbContext _dbContext;
        public RepositoryBase(TestFoodShopWebsiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetQueryable
            (
                Expression<Func<TEntity, bool>>? filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? sortOrder = null,
                string? include = null
            )
        {
            IQueryable<TEntity> result = _dbContext.Set<TEntity>();

            if (filter is not null)
            {
                result = result.Where(filter);
            }

            //if(filter is not null)
            //{ 
            //    foreach (Expression<Func<TEntity, bool>> filterItems in filter)
            //    {
            //        if (filterItems is not null)
            //            result = result.Where(filterItems);
            //    }
            //}

            if (sortOrder is not null)
            {
                result = sortOrder(result);
            }


            if (!string.IsNullOrEmpty(include))
            {
                foreach (var item in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    result = result.Include(item);
                }
            }

            return result;
        }

        public IQueryable<TEntity> GetAll(
                         Expression<Func<TEntity, bool>>? filter = null,
                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                         string? include = null)
        {
            return GetQueryable(filter, orderBy, include);
        }

        public IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string? include = null
            )
        {
            return GetQueryable(filter, orderBy, include);
        }

        public TEntity? GetSingle(Expression<Func<TEntity, bool>>? filter = null, string? include = null)
        {
            return GetQueryable(filter, null, include).SingleOrDefault()
                ?? null; // throw new KeyNotFoundException("Datensatz konnte nicht eindeutig gefunden werden!");
        }

        public TEntity Create(TEntity newModel)
        {
            _dbContext.Add(newModel);

            _dbContext.SaveChanges(); //Eigene Änderung, weil keine Exception

            //try
            //{
            //    _dbContext.SaveChanges();
            //}
            //catch (InvalidOperationException ex)
            //{
            //    throw new RepositoryCreateException("Methode TEntity Create(TEntity) ist fehlgeschlagen!", ex);
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    throw new RepositoryCreateException("Methode TEntity Create(TEntity) ist fehlgeschlagen!", ex);
            //}
            //catch (DbUpdateException ex)
            //{
            //    throw new RepositoryCreateException("Methode TEntity Create(TEntity) ist fehlgeschlagen!", ex);
            //}
            return newModel;
        }

        public bool Edit(TEntity newModel)
        {
            try
            {
                _dbContext.Update(newModel);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            //try
            //    {
            //        _dbContext.SaveChanges();
            //        return newModel;
            //    }
            //    catch (InvalidOperationException ex)
            //    {
            //        throw new RepositoryUpdateException("Methode TEntity Edit(TEntity) ist fehlgeschlagen!", ex);
            //    }
            //    catch (DbUpdateConcurrencyException ex)
            //    {
            //        throw new RepositoryUpdateException("Methode TEntity Edit(TEntity) ist fehlgeschlagen!", ex);
            //    }
            //    catch (DbUpdateException ex)
            //    {
            //        throw new RepositoryUpdateException("Methode TEntity Edit(TEntity) ist fehlgeschlagen!", ex);
            //    }
        }

        public bool Delete(TEntity model)
        {
            _dbContext.Remove(model);
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }

}