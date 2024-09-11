using System.Reflection;

namespace Zylo.Infrastructure;

public static class InfrastructureReference
{
    public static readonly Assembly Assembly = typeof(InfrastructureReference).Assembly;
}
