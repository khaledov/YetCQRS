﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using JITDispatcher.Commands;
using JITDispatcher.Dispatchers;
using JITDispatcher.Events;
using JITDispatcher.Queries;

namespace JITDispatcher.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJITDispatcher(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTransient<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<IQueryDispatcher, QueryDispatcher>();
        services.AddTransient<IEventDispatcher, EventDispatcher>();
        services.AddTransient<IDispatcher, Dispatcher>();

        // Get all non-abstract classes in the specified assemblies
        var allTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract);

        foreach (var type in allTypes)
        {
            // Get interfaces implemented by the type that match the generic interface
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IEventHandler<>)||
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(ICommandValidator<>));

            foreach (var interfaceType in interfaces)
            {
                // Register the type with the closed generic interface
                var serviceDescriptor = new ServiceDescriptor(interfaceType, type, ServiceLifetime.Transient);
                services.Add(serviceDescriptor);
            }
        }
        return services;
    }
}
