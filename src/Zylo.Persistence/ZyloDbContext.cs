using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Zylo.Application.Abstractions.Data;
using Zylo.Domain.Events;
using Zylo.Domain.Friendships;
using Zylo.Domain.Invitations;
using Zylo.Domain.Notifications;
using Zylo.Domain.Users;
using Zylo.Persistence.Outbox;

namespace Zylo.Persistence;

public sealed class ZyloDbContext : DbContext, IUnitOfWork, IDbContext
{
    public ZyloDbContext(DbContextOptions options) 
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<EmailVerificationCode> EmailVerificationCodes { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<Invitation> Invitations { get; set; }

    public DbSet<Friendship> Friendships { get; set; }

    public DbSet<FriendRequest> FriendRequests { get; set; }

    public DbSet<Attendee> Attendees { get; set; }

    public DbSet<Event> Events { get; set; }

    public DbSet<PersonalEvent> PersonalEvents { get; set; }

    public DbSet<GroupEvent> GroupEvents { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(PersistenceReference.Assembly);
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return (await Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }
}
