using Kuk.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Kuk.Data.Common
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        DbSet<TEntity> Entities { get; }
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }

        void Add(TEntity entity, bool saveNow = true);
        Task AddAsync(TEntity entity, bool saveNow = true);
        void AddRange(IEnumerable<TEntity> entities, bool saveNow = true);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default,
            bool saveNow = true);

        void Attach(TEntity entity);
        void Delete(TEntity entity, bool saveNow = true);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true);
        void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true);

        void Detach(TEntity entity);
        TEntity GetById(params object[] ids);
        ValueTask<TEntity?> GetByIdAsync(params object[] ids);

        void Update(TEntity entity, bool saveNow = true, params object[] ids);

        Task UpdateAsync(TEntity entity, bool saveNow = true, params object[] ids);
    }
}
