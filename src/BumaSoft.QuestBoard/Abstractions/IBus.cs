namespace BumaSoft.QuestBoard.Abstractions;

public interface IBus
{
    public Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);
    public Task<TResponse> SendAsync<TMessage, TResponse>(TMessage message, CancellationToken cancellationToken = default);
}
