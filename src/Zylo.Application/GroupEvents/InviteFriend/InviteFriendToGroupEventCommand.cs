using Zylo.Application.Abstractions.Messaging;

namespace Zylo.Application.GroupEvents.InviteFriend;

public sealed record InviteFriendToGroupEventCommand(Guid GroupEventId, Guid FriendId) : ICommand;
