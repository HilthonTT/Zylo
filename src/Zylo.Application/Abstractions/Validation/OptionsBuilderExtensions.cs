﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Zylo.Application.Abstractions.Validation;

public static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(
        this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        builder.Services.AddSingleton<IValidateOptions<TOptions>>(
            serviceProvider => new FluentValidateOptions<TOptions>(serviceProvider, builder.Name));

        return builder;
    }
}
