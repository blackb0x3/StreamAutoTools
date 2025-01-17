﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace StreamInstruments.Hubs.Api.Domain.Infrastructure;

public static class DomainInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddMediatR(DomainAssembly);
        services.AddAutoMapper(DomainAssembly);
    }

    private static Assembly DomainAssembly => typeof(DomainInstaller).Assembly;
}