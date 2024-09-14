using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zylo.Domain.Friendships;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Configurations;

internal sealed class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.ToTable("friend_requests");

        builder.HasKey(f => f.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.FriendId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(f => f.Accepted).HasDefaultValue(false);

        builder.Property(f => f.Rejected).HasDefaultValue(false);

        builder.Property(f => f.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(f => !f.IsDeleted);
    }
}
