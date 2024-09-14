﻿namespace Zylo.Infrastructure.Notifications.Options;

internal sealed class EmailOptions
{
    public const string ConfigurationSection = "Email";

    public string SenderEmail { get; set; } = string.Empty;

    public string Sender { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }
}
