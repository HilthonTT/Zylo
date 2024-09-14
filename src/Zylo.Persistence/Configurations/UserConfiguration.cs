using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zylo.Domain.Users;
using Zylo.Domain.Users.ValueObjects;

namespace Zylo.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.OwnsOne(u => u.FirstName, builder =>
        {
            builder.WithOwner();

            builder.Property(firstName => firstName.Value)
                .HasColumnName("first_name")
                .HasMaxLength(FirstName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(u => u.LastName, builder =>
        {
            builder.WithOwner();

            builder.Property(lastName => lastName.Value)
                .HasColumnName("last_name")
                .HasMaxLength(LastName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(u => u.Email, builder =>
        {
            builder.WithOwner();

            builder.Property(email => email.Value)
                .HasColumnName("email")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();

            builder.HasIndex(email => email.Value).IsUnique();
        });

        builder.Property(u => u.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(u => !u.IsDeleted);

        builder.Ignore(u => u.FullName);
    }
}
