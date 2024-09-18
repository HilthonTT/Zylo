using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SharedKernel;
using Zylo.Application.Abstractions.Data;
using Zylo.Domain.Events;
using Zylo.Domain.Friendships;
using Zylo.Domain.Invitations;
using Zylo.Domain.Notifications;
using Zylo.Domain.Users;
using Zylo.Persistence.Interceptors;
using Zylo.Persistence.Outbox;
using Zylo.Persistence.Repositories;

namespace Zylo.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<InsertOutboxMessagesInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<UpdateAuditableInterceptor>();

        string? connectionString = configuration.GetConnectionString("Database");

        Ensure.NotNullOrEmpty(connectionString, nameof(connectionString));

        services.AddDbContext<ZyloDbContext>(
            (sp, options) => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(
                    sp.GetRequiredService<InsertOutboxMessagesInterceptor>(),
                    sp.GetRequiredService<SoftDeleteInterceptor>(),
                    sp.GetRequiredService<UpdateAuditableInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ZyloDbContext>());

        services.AddScoped<IDbContext>(sp => sp.GetRequiredService<ZyloDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<IGroupEventRepository, GroupEventRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IEmailVerificationCodeRepository, EmailVerificationCodeRepository>();
        services.AddScoped<IPersonalEventRepository, PersonalEventRepository>();

        services.AddSingleton<IDbConnectionFactory>(_ =>
            new DbConnectionFactory(new NpgsqlDataSourceBuilder(connectionString).Build()));

        return services;
    }
}
