using CoinApp.Domain.Interfaces;
using CoinApp.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoinApp.Infrastructure.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        //T GetById(int id, params string[] includes);
        T GetSingleBySpec(ISpecification<T> spec);
        IEnumerable<T> ListAll(params string[] includes);
        IEnumerable<T> List(ISpecification<T> spec);
        T Add(T entity);
        Task<T> AddAsync(T entity);

        void Update(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> list);

        List<T> ListByCriteria(Expression<Func<T, bool>> criteria, params string[] includes);
        List<T> ListByCriteriaByPaging(int skip, int take, Expression<Func<T, bool>> criteria, params string[] includes);

        //T GetSingleByCriteria(Expression<Func<T, bool>> criteria);
        T GetSingleByCriteria(Expression<Func<T, bool>> criteria, params string[] includes);
        bool Any(Expression<Func<T, bool>> criteria);

        //T GetLastEntity();

        void AddEntity(T entity);
        void AddRange(IEnumerable<T> entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        void Save();

        int CountByCriteria(Expression<Func<T, bool>> criteria);

        IQueryable<T> CreateQuery(params string[] includes);

        void DeleteRange(IEnumerable<T> entities);
        void UpdateRange(IEnumerable<T> entities);
        Task UpdateRangeAsync(IEnumerable<T> entities);
    }
}