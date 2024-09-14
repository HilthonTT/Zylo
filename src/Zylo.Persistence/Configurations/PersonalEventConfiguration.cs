using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zylo.Domain.Events;

namespace Zylo.Persistence.Configurations;

internal sealed class PersonalEventConfiguration : IEntityTypeConfiguration<PersonalEvent>
{
    public void Configure(EntityTypeBuilder<PersonalEvent> builder)
    {
        builder.Property(pe => pe.Processed)
            .IsRequired()
            .HasDefaultValue(false);
    }
}
