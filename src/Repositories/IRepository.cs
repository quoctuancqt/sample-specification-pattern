namespace src.Repositories
{
    using src.Domain;
    using src.Specifications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<T> where T : EntityBase
    {
        IQueryable<T> FindAll(Expression<Func<T, bool>> pression = null);

        IQueryable<T> FindAllWithSpecification(ISpecification<T> spec);

        T FindBy(int id);

        Task<T> FindByAsync(int id);

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

    }
}
