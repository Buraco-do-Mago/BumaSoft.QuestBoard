using System.Reflection;
using BumaSoft.QuestBoard.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BumaSoft.QuestBoard.DependencyInjection;

public class QuestBoardConfiguration(IServiceCollection services)
{
    public QuestBoardConfiguration AddHandler<TMessage, THandler>()
        where THandler : class, IHandler<TMessage>
    {
        services.AddScoped<IHandler<TMessage>, THandler>();
        return this;
    }

    public QuestBoardConfiguration AddHandler<TMessage, TResponse, THandler>()
        where THandler : class, IHandler<TMessage, TResponse>
    {
        services.AddScoped<IHandler<TMessage, TResponse>, THandler>();
        return this;
    }

    public QuestBoardConfiguration AddHandlerAsync<TMessage, THandler>()
        where THandler : class, IHandlerAsync<TMessage>
    {
        services.AddScoped<IHandlerAsync<TMessage>, THandler>();
        return this;
    }

    public QuestBoardConfiguration AddHandlerAsync<TMessage, TResponse, THandler>()
        where THandler : class, IHandlerAsync<TMessage, TResponse>
    {
        services.AddScoped<IHandlerAsync<TMessage, TResponse>, THandler>();
        return this;
    }

    public QuestBoardConfiguration AddHandlersFromAsseblies(Assembly[] assemblies)
    {
        var handlerTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Distinct()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType &&
                        (x.Interface.GetGenericTypeDefinition() == typeof(IHandler<>) ||
                         x.Interface.GetGenericTypeDefinition() == typeof(IHandler<,>) ||
                         x.Interface.GetGenericTypeDefinition() == typeof(IHandlerAsync<>) ||
                         x.Interface.GetGenericTypeDefinition() == typeof(IHandlerAsync<,>)))
            .ToList();

        foreach (var handler in handlerTypes)
            services.AddScoped(handler.Interface, handler.Type);

        return this;
    }

    public QuestBoardConfiguration AddHandlersFromAssembly(Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(x => x.Interface.IsGenericType &&
                        (x.Interface.GetGenericTypeDefinition() == typeof(IHandler<>) ||
                         x.Interface.GetGenericTypeDefinition() == typeof(IHandler<,>) ||
                         x.Interface.GetGenericTypeDefinition() == typeof(IHandlerAsync<>) ||
                         x.Interface.GetGenericTypeDefinition() == typeof(IHandlerAsync<,>)))
            .ToList();

        foreach (var handler in handlerTypes)
            services.AddScoped(handler.Interface, handler.Type);

        return this;
    }
}
