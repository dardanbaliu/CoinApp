using CoinApp.Domain.Interfaces;
using CoinApp.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoinApp.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T>, IAsyncRepository<T> where T : Entity
    {
        protected readonly CoinAppContext _dbContext;

        public EfRepository(CoinAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public virtual T GetById(int id, params string[] includes)
        //{
        //    IQueryable<T> query = _dbContext.Set<T>();

        //    if (includes != null)
        //    {
        //        int count = includes.Length;
        //        for (int index = 0; index < count; index++)
        //        {
        //            query = query.Include(includes[index]);
        //        }
        //    }
        //    return query.FirstOrDefault(t => t.Id.Equals(id));
        //}

        public T GetSingleBySpec(ISpecification<T> spec)
        {
            return List(spec).FirstOrDefault();
        }

        public virtual async Task<T> GetByIdAsync(int id, params string[] includes)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public IEnumerable<T> ListAll(params string[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                int count = includes.Length;
                for (int index = 0; index < count; index++)
                {
                    query = query.Include(includes[index]);
                }
            }

            return query.AsEnumerable();

            //_dbContext.Set<T>().AsEnumerable();
        }

        public IQueryable<T> CreateQuery(params string[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                int count = includes.Length;
                for (int index = 0; index < count; index++)
                {
                    query = query.Include(includes[index]);
                }
            }

            return query;
        }

        public T GetSingleByCriteria(Expression<Func<T, bool>> criteria, params string[] includes)
        {
            return ListByCriteria(criteria, includes).FirstOrDefault();
        }

        public bool Any(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.Any(criteria);
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public List<T> ListByCriteria(Expression<Func<T, bool>> criteria, params string[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                int count = includes.Length;
                for (int index = 0; index < count; index++)
                {
                    query = query.Include(includes[index]);
                }
            }

            return query.Where(criteria).ToList();
        }

        public List<T> ListByCriteriaByPaging(int skip, int take, Expression<Func<T, bool>> criteria, params string[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                int count = includes.Length;
                for (int index = 0; index < count; index++)
                {
                    query = query.Include(includes[index]);
                }
            }

            return query.Where(criteria).Skip(skip).Take(take).ToList();
        }


        public int CountByCriteria(Expression<Func<T, bool>> criteria)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.Where(criteria).Count();
        }

        public IEnumerable<T> List(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult
                            .Where(spec.Criteria)
                            .AsEnumerable();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                            .Where(spec.Criteria)
                            .ToListAsync();
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }


        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        public async Task AddRangeAsync(IEnumerable<T> list)
        {
            await _dbContext.Set<T>().AddRangeAsync(list);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteRangeAsync(IEnumerable<T> list)
        {
            _dbContext.Set<T>().RemoveRange(list);
            await _dbContext.SaveChangesAsync();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        //public virtual T GetLastEntity()
        //{
        //    return _dbContext.Set<T>().OrderByDescending(t => t.Id).FirstOrDefault();
        //}

        public void AddEntity(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> list)
        {
            _dbContext.Set<T>().AddRange(list);
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            _dbContext.SaveChanges();
        }
        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}
