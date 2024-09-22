using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using System.Text;
using Zylo.Application.Abstractions.Authentication;
using Zylo.Application.Abstractions.Caching;
using Zylo.Application.Abstractions.Events;
using Zylo.Application.Abstractions.Notifications;
using Zylo.Application.Abstractions.Validation;
using Zylo.Infrastructure.Authentication;
using Zylo.Infrastructure.Authentication.Options;
using Zylo.Infrastructure.Caching;
using Zylo.Infrastructure.Emails;
using Zylo.Infrastructure.Events;
using Zylo.Infrastructure.Notifications;
using Zylo.Infrastructure.Notifications.Options;
using Zylo.Infrastructure.Outbox;
using Zylo.Infrastructure.Time;

namespace Zylo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();
        services.AddMessaging();
        services.AddAuthenticationInternal();
        services.AddAuthorizationInternal();
        services.AddNotifications();

        services.AddBackgroundJobs(configuration);
        services.AddCaching(configuration);
        services.AddHealthChecks(configuration);

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(InfrastructureReference.Assembly, includeInternalTypes: true);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddTransient<INotificationService, NotificationService>();

        return services;
    }

    private static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        services.AddOptionsWithFluentValidation<EmailOptions>(EmailOptions.ConfigurationSection);

        using var serviceProvider = services.BuildServiceProvider();

        var emailOptions = serviceProvider.GetRequiredService<IOptions<EmailOptions>>().Value;

        services
            .AddFluentEmail(emailOptions.SenderEmail, emailOptions.Sender)
            .AddSmtpSender(emailOptions.Host, emailOptions.Port);

        services.AddHostedService<EmailVerificationCodeProcessorJob>();

        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryMessageQueue>();
        services.AddSingleton<IEventBus, EventBus>();

        services.AddHostedService<IntegrationEventProcessorJob>();

        return services;
    }

    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        string? redisConnectionString = configuration.GetConnectionString("Cache");
        Ensure.NotNullOrEmpty(redisConnectionString, nameof(redisConnectionString));

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = redisConnectionString);

        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString, nameof(connectionString));

        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(options => 
                options.UseNpgsqlConnection(connectionString)));

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1));

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString, nameof(connectionString));

        string? redisConnectionString = configuration.GetConnectionString("Cache");
        Ensure.NotNullOrEmpty(redisConnectionString, nameof(redisConnectionString));


        services.AddHealthChecks()
            .AddNpgSql(connectionString)
            .AddRedis(redisConnectionString);

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddOptionsWithFluentValidation<JwtOptions>(JwtOptions.ConfigurationSection);

        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        var jwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }
}
