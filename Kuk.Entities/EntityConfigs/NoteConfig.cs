using Kuk.Entities.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kuk.Entities.EntityConfigs
{
    public class NoteConfig : IEntityTypeConfiguration<NoteEntity>
    {
        public void Configure(EntityTypeBuilder<NoteEntity> builder)
        {
            builder.ToTable("Note", "Model");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.TextBody).IsRequired();
            builder.Property(p => p.CreateDateTime).IsRequired();
        }
    }
}
