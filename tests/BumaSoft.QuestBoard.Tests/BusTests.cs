using BumaSoft.QuestBoard.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BumaSoft.QuestBoard.Tests;

[TestFixture]
public class BusTests
{
    private ServiceProvider serviceProvider;
    private IServiceScope scope;

    [OneTimeSetUp]
    public void OneTimeSetUp() =>
        serviceProvider = new ServiceCollection()
            .AddScoped<IHandler<TestRequest, TestResponse>, TestHandler>()
            .AddScoped<IHandlerAsync<TestAsyncRequest, TestResponse>, TestAsyncHandler>()
            .AddScoped<Bus>()
            .BuildServiceProvider();

    [SetUp]
    public void SetUp() => scope = serviceProvider.CreateScope();

    [Test]
    public async Task Bus_ShouldSendMessageToRegisteredHandlerAsync()
    {
        var bus = scope.ServiceProvider.GetRequiredService<Bus>();

        var request = new TestRequest { Message = "Hello, Bus!" };
        var response = await bus.SendAsync<TestRequest, TestResponse>(request);

        Assert.That(response.ResponseMessage, Is.EqualTo("Handled: Hello, Bus!"));
    }

    [TearDown]
    public void TearDown() => scope.Dispose();

    [OneTimeTearDown]
    public void OneTimeTearDown() => serviceProvider.Dispose();
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

public class TestAsyncRequest
{
    public required string Message { get; set; }
}

public class TestAsyncHandler : IHandlerAsync<TestAsyncRequest, TestResponse>
{
    public async Task<TestResponse> Handle(TestAsyncRequest message, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        return new TestResponse { ResponseMessage = $"Handled asynchronously: {message.Message}" };
    }
}
