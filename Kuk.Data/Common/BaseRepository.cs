using Kuk.Common.Utilities;
using Kuk.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Kuk.Data.Common
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IEntity
    {
        protected readonly KukDbContext DbContext;

        public BaseRepository(KukDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.Set<TEntity>();
        }

        public DbSet<TEntity> Entities { get; }
        public virtual IQueryable<TEntity> Table => Entities;
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        #region Async Method

        public virtual ValueTask<TEntity?> GetByIdAsync(params object[] ids)
        {
            return Entities.FindAsync(ids);
        }

        public virtual async Task AddAsync(TEntity entity,
            bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Add(entity);
            if (saveNow)
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.AddRange(entities);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TEntity entity, bool saveNow = true, params object[] ids)
        {
            Assert.NotNull(entity, nameof(entity));
            var existing = await Entities.FindAsync(ids).ConfigureAwait(false);
            Assert.NotNull(existing, nameof(existing));
            DbContext.Entry(existing).CurrentValues.SetValues(entity);
            if (saveNow)
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default,
            bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities,
            bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion

        #region Sync Methods

        public virtual TEntity GetById(params object[] ids)
        {
            return Entities.Find(ids);
        }

        public virtual void Add(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Add(entity);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.AddRange(entities);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity, bool saveNow = true, params object[] ids)
        {
            Assert.NotNull(entity, nameof(entity));
            var existing = Entities.Find(ids);
            Assert.NotNull(existing, nameof(existing));
            DbContext.Entry(existing).CurrentValues.SetValues(entity);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void Delete(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
                DbContext.SaveChanges();
        }

        #endregion

        #region Attach & Detach

        public virtual void Detach(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            var entry = DbContext.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Detached;
        }

        public virtual void Attach(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            if (DbContext.Entry(entity).State == EntityState.Detached)
                Entities.Attach(entity);
        }

        #endregion

    }
}
