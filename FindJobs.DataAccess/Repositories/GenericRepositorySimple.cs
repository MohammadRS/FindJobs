using System;
using FindJobs.DataAccess.Persistence;
using FindJobs.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindJobs.DataAccess.Repositories
{
    public class GenericRepositorySimple<TEntity> : IGenericRepositorySimple<TEntity> where
        TEntity : class
    {
        private readonly FindJobsDbContext context;
        private DbSet<TEntity> DbSet { get; }
        public GenericRepositorySimple(FindJobsDbContext context)
        {
            this.context = context;
            this.DbSet = context.Set<TEntity>();
        }
        public IQueryable<TEntity> GetEntities()
        {
            return DbSet.AsQueryable();
        }
        public async Task AddEntity(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }
        public void AddEntityRange(IEnumerable<TEntity> entities, int steps = 40000)
        {
            for (var i = 0; i < entities.Count(); i += steps)
            {
                var res = entities.Skip(i).Take(steps);
                DbSet.AddRange(res);
                SaveChange();
            }
        }
        public void RemoveEntity(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        public void RemoveEntityRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void UpdateEntityRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }
        public void UpdateEntity(TEntity entity)
        {
            DbSet.Update(entity);
        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void SaveChange()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            this.context?.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            context?.DisposeAsync().ConfigureAwait(false);
            return new ValueTask();
        }
    }
}
