using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindJobs.Domain.Repositories
{
    public interface IGenericRepositorySimple<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetEntities();
        Task AddEntity(TEntity entity);
        void AddEntityRange(IEnumerable<TEntity> entities, int steps = 40000);
        void UpdateEntity(TEntity entity);
        void RemoveEntity(TEntity entity);
        void RemoveEntityRange(IEnumerable<TEntity> entities);
        void UpdateEntityRange(IEnumerable<TEntity> entities);

        Task SaveAsync();
        void SaveChange();
    }
}
