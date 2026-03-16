using BumaSoft.QuestBoard.Abstractions;
using BumaSoft.QuestBoard.Tests.Mocks;
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
            .AddScoped<IHandler<TestVoidRequest>, TestVoidHandler>()
            .AddScoped<IHandlerAsync<TestVoidAsyncRequest>, TestVoidAsyncHandler>()
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

    [Test]
    public async Task Bus_ShouldSendMessageToRegisteredAsyncHandlerAsync()
    {
        var bus = scope.ServiceProvider.GetRequiredService<Bus>();

        var request = new TestAsyncRequest { Message = "Hello, Async Bus!" };
        var response = await bus.SendAsync<TestAsyncRequest, TestResponse>(request);

        Assert.That(response.ResponseMessage, Is.EqualTo("Handled asynchronously: Hello, Async Bus!"));
    }

    [Test]
    public async Task Bus_ShouldHandleVoidMessageWithRegisteredHandlerAsync()
    {
        var bus = scope.ServiceProvider.GetRequiredService<Bus>();

        var request = new TestVoidAsyncRequest { Message = "Hello, Void Bus!" };

        Assert.DoesNotThrowAsync(() => bus.SendAsync(request));
    }

    [Test]
    public async Task Bus_ShouldHandleVoidMessageWithRegisteredHandler()
    {
        var bus = scope.ServiceProvider.GetRequiredService<Bus>();

        var request = new TestVoidRequest { Message = "Hello, Void Bus!" };

        Assert.DoesNotThrowAsync(() => bus.SendAsync(request));
    }

    [Test]
    public async Task Bus_ShouldThrowException_WhenNoHandlerRegisteredAsync()
    {
        var bus = scope.ServiceProvider.GetRequiredService<Bus>();

        var request = (object)new { Message = "Hello, No Handler!" };

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await bus.SendAsync(request));
        Assert.That(ex.Message, Does.Contain("No handler was registered for message 'Object'."));
    }

    [Test]
    public async Task Bus_ShouldThrowException_WhenNoHandlerWithResponseRegisteredAsync()
    {
        var bus = scope.ServiceProvider.GetRequiredService<Bus>();

        var request = (object)new { Message = "Hello, No Handler!" };

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await bus.SendAsync<object, string>(request));
        Assert.That(ex.Message, Does.Contain("No handler was registered for message 'Object' with response 'String'."));
    }

    [TearDown]
    public void TearDown() => scope.Dispose();

    [OneTimeTearDown]
    public void OneTimeTearDown() => serviceProvider.Dispose();
}
