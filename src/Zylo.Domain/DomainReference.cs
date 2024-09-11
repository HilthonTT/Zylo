using System.Reflection;

namespace Zylo.Domain;

public static class DomainReference
{
    public static readonly Assembly Assembly = typeof(DomainReference).Assembly;
}
