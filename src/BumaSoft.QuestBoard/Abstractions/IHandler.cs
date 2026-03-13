namespace BumaSoft.QuestBoard.Abstractions;

public interface IHandler<TMessage, TResponse>
{
    public TResponse Handle(TMessage message);
}

public interface IHandler<TMessage>
{
    public void Handle(TMessage message);
}

public interface IHandlerAsync<TMessage, TResponse>
{
    public Task<TResponse> Handle(TMessage message);
}

public interface IHandlerAsync<TMessage>
{
    public Task Handle(TMessage message);
}
