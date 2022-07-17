using Kuk.Data;
using Kuk.Entities.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace Kuk.UnitTests.Mock
{
    public class MockKukDbContext : KukDbContext
    {
        static DbContextOptions<KukDbContext> options = new DbContextOptionsBuilder<KukDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;
        public MockKukDbContext() : base(options)
        {
        }

        public KukDbContext MockAndSeedDbContext()
        {
            var context = new KukDbContext(options);
            context.Notes.Add(new NoteEntity { Title = "text1", TextBody = "text body 1", CreateDateTime = DateTime.Now });
            context.Notes.Add(new NoteEntity { Title = "text2", TextBody = "text body 2", CreateDateTime = DateTime.Now });

            context.SaveChanges();
            return context;
        }
    }
}
