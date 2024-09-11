using System.Reflection;
using Zylo.Api;
using Zylo.Application;
using Zylo.Domain;
using Zylo.Infrastructure;
using Zylo.Persistence;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = DomainReference.Assembly;
    protected static readonly Assembly ApplicationAssembly = ApplicationReference.Assembly;
    protected static readonly Assembly InfrastructureAssembly = InfrastructureReference.Assembly;
    protected static readonly Assembly PersistenceAssembly = PersistenceReference.Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}
