using BumaSoft.QuestBoard.Abstractions;

namespace BumaSoft.QuestBoard.Tests.Mocks;

public class FaultyBus : IBus
{
    public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TResponse> SendAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
