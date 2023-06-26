using FindJobs.DataAccess.Entities;
using FindJobs.DataAccess.Persistence;
using FindJobs.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FindJobs.DataAccess.Repositories
{
    public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly FindJobsDbContext context;
        private DbSet<TEntity> DbSet { get; }

        public GenericRepository(FindJobsDbContext context)
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
            entity.LastUpdateDate = DateTime.Now;
            entity.CreateDate = DateTime.Now;
            entity.IsDeleted = false;
            await DbSet.AddAsync(entity);
        }
       public async Task AddEntityRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                entity.LastUpdateDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                entity.IsDeleted = false;
            }
           await DbSet.AddRangeAsync(entities);
        }
        public void RemoveEntity(TEntity entity)
        {
            entity.IsDeleted = true;
            UpdateEntity(entity);
        }
        public void DeleteEntity(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        public void RemoveEntityRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            UpdateEntityRange(entities);
        }

        public void UpdateEntity(TEntity entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            DbSet.Update(entity);
        }
        public void UpdateEntityRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.LastUpdateDate = DateTime.Now;
            }
            
            DbSet.UpdateRange(entities);
        }
        public async Task SaveChange()
        {
            await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context?.Dispose();
        }

       
    }
}