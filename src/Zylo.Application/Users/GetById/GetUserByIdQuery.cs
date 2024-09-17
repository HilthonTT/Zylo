using Zylo.Application.Abstractions.Messaging;
using Zylo.Contracts.Users;

namespace Zylo.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
