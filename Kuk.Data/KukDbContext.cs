using Kuk.Entities.EntityConfigs;
using Kuk.Entities.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace Kuk.Data
{
    public class KukDbContext : DbContext
    {
        public KukDbContext()
        {
        }
        public KukDbContext(DbContextOptions<KukDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new NoteConfig());

            base.OnModelCreating(builder);
        }

        public DbSet<NoteEntity> Notes { get; set; }
    }
}
