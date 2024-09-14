using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zylo.Domain.Events;
using Zylo.Domain.Events.ValueObjects;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Configurations;

internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasDiscriminator(e => e.Type)
            .HasValue<PersonalEvent>(EventType.PersonalEvent)
            .HasValue<GroupEvent>(EventType.GroupEvent);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder.OwnsOne(e => e.Name, builder =>
        {
            builder.WithOwner();

            builder.Property(name => name.Value)
                .HasColumnName("name")
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(e => e.Category, builder =>
        {
            builder.WithOwner();

            builder.Property(category => category.Id)
                .HasColumnName("category")
                .IsRequired();
        });

        builder.Property(e => e.Cancelled).HasDefaultValue(false);

        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
