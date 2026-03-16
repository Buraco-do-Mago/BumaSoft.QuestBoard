using BumaSoft.QuestBoard.Abstractions;

namespace BumaSoft.QuestBoard.Tests.Mocks;

public class TestHandler : IHandler<TestRequest, TestResponse>
{
    public TestResponse Handle(TestRequest message) => new() { ResponseMessage = $"Handled: {message.Message}" };
}


public class TestAsyncHandler : IHandlerAsync<TestAsyncRequest, TestResponse>
{
    public async Task<TestResponse> Handle(TestAsyncRequest message, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        return new TestResponse { ResponseMessage = $"Handled asynchronously: {message.Message}" };
    }
}

public class TestVoidHandler : IHandler<TestVoidRequest>
{
    public void Handle(TestVoidRequest message)
    {
        // Simulate handling the message
    }
}


public class TestVoidAsyncHandler : IHandlerAsync<TestVoidAsyncRequest>
{
    public async Task Handle(TestVoidAsyncRequest message, CancellationToken cancellationToken = default)
    {
        // Simulate handling the message asynchronously
        await Task.Delay(100, cancellationToken);
    }
}
