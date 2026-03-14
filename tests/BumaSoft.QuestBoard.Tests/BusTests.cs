using BumaSoft.QuestBoard.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BumaSoft.QuestBoard.Tests;

[TestFixture]
public class BusTests
{
    private IServiceScope scope;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IHandler<TestRequest, TestResponse>, TestHandler>()
            .AddScoped<IHandlerAsync<TestRequest, TestResponse>, TestAsyncHandler>()
            .AddScoped<Bus>()
            .BuildServiceProvider();

        scope = serviceProvider.CreateScope();
    }

    [Test]
    public async Task Bus_ShouldSendMessageToRegisteredHandlerAsync()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IHandler<TestRequest, TestResponse>, TestHandler>()
            .AddScoped<Bus>()
            .BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var bus = scope.ServiceProvider.GetRequiredService<Bus>();

        var request = new TestRequest { Message = "Hello, Bus!" };
        var response = await bus.SendAsync<TestRequest, TestResponse>(request);

        Assert.That(response.ResponseMessage, Is.EqualTo("Handled: Hello, Bus!"));
    }
}

public class TestRequest
{
    public required string Message { get; set; }
}

public class TestResponse
{
    public required string ResponseMessage { get; set; }
}

public class TestHandler : IHandler<TestRequest, TestResponse>
{
    public TestResponse Handle(TestRequest message)
    {
        return new TestResponse { ResponseMessage = $"Handled: {message.Message}" };
    }
}

public class TestAsyncHandler : IHandlerAsync<TestRequest, TestResponse>
{
    public async Task<TestResponse> Handle(TestRequest message, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        return new TestResponse { ResponseMessage = $"Handled asynchronously: {message.Message}" };
    }
}
