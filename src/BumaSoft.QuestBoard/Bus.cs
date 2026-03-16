using Microsoft.Extensions.DependencyInjection;
using BumaSoft.QuestBoard.Abstractions;

namespace BumaSoft.QuestBoard;

public class Bus(IServiceProvider serviceProvider) : IBus
{
    public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
    {
        var asyncHandler = serviceProvider.GetService<IHandlerAsync<TMessage>>();
        if (asyncHandler is not null)
            return asyncHandler.Handle(message, cancellationToken);

        var handler = serviceProvider.GetService<IHandler<TMessage>>();
        if (handler is not null)
        {
            handler.Handle(message);
            return Task.CompletedTask;
        }

        throw new InvalidOperationException(GetHandlerNotFoundMessage(typeof(TMessage)));
    }

    public Task<TResponse> SendAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken = default)
    {
        var asyncHandler = serviceProvider.GetService<IHandlerAsync<TMessage, TResponse>>();
        if (asyncHandler is not null)
            return asyncHandler.Handle(message, cancellationToken);

        var handler = serviceProvider.GetService<IHandler<TMessage, TResponse>>();
        if (handler is not null)
            return Task.FromResult(handler.Handle(message));

        throw new InvalidOperationException(GetHandlerNotFoundMessage(typeof(TMessage), typeof(TResponse)));
    }

    private static string GetHandlerNotFoundMessage(Type messageType, Type? responseType = null) =>
        $"No handler was registered for message '{messageType.Name}'{(responseType is null ? string.Empty : $" with response '{responseType.Name}'")}. Did you forget to call AddQuestBoard() or register the handle?";
}
