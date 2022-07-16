using Kuk.Data.Common;
using Kuk.Data.IRepositories;
using Kuk.Entities.EntityModels;

namespace Kuk.Data.Repositories
{
    public class NoteRepository : BaseRepository<NoteEntity> , INoteRepository
    {
        public NoteRepository(KukDbContext dbContext) : base(dbContext)
        {
        }
    }
}
