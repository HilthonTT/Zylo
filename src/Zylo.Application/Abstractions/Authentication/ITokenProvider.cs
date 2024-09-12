using Zylo.Domain.Users;

namespace Zylo.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user);
}
