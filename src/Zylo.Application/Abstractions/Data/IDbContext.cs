using Microsoft.EntityFrameworkCore;
using Zylo.Domain.Events;
using Zylo.Domain.Friendships;
using Zylo.Domain.Invitations;
using Zylo.Domain.Notifications;
using Zylo.Domain.Users;

namespace Zylo.Application.Abstractions.Data;

public interface IDbContext
{
    public DbSet<User> Users { get;  }

    public DbSet<Notification> Notifications { get;  }

    public DbSet<Invitation> Invitations { get;  }

    public DbSet<Friendship> Friendships { get; }

    public DbSet<FriendRequest> FriendRequests { get; }

    public DbSet<Attendee> Attendees { get; }

    public DbSet<Event> Events { get; }

    public DbSet<PersonalEvent> PersonalEvents { get; }

    public DbSet<GroupEvent> GroupEvents { get; }

}
