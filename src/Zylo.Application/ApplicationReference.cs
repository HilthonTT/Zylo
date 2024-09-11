using System.Reflection;

namespace Zylo.Application;

public static class ApplicationReference
{
    public static readonly Assembly Assembly = typeof(ApplicationReference).Assembly;
}
