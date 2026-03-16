using BumaSoft.QuestBoard.Abstractions;
using BumaSoft.QuestBoard.DependencyInjection;
using BumaSoft.QuestBoard.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace BumaSoft.QuestBoard.Tests;

[TestFixture]
public class QuestBoardDependencyInjectionTests
{
    [Test]
    public void QuestBoardDependencyInjection_ShouldRegisterBus()
    {
        var services = new ServiceCollection();
        services.AddQuestBoard();
        using var scope = services.BuildServiceProvider().CreateScope();
        var bus = scope.ServiceProvider.GetService<Bus>();
        Assert.That(bus, Is.Not.Null);
        var busInterface = scope.ServiceProvider.GetService<IBus>();
        Assert.That(busInterface, Is.Not.Null);
        Assert.That(busInterface, Is.SameAs(bus));
    }

    [Test]
    public void QuestBoardDependencyInjection_ShouldRegisterHandler()
    {
        var services = new ServiceCollection();
        services.AddQuestBoard(config => config.AddHandler<TestVoidRequest, TestVoidHandler>());
        using var scope = services.BuildServiceProvider().CreateScope();
        var handler = scope.ServiceProvider.GetService<IHandler<TestVoidRequest>>();
        Assert.That(handler, Is.Not.Null);
    }

    [Test]
    public void QuestBoardDependencyInjection_ShouldRegisterAsyncHandler()
    {
        var services = new ServiceCollection();
        services.AddQuestBoard(config => config.AddHandlerAsync<TestVoidAsyncRequest, TestVoidAsyncHandler>());
        using var scope = services.BuildServiceProvider().CreateScope();
        var handler = scope.ServiceProvider.GetService<IHandlerAsync<TestVoidAsyncRequest>>();
        Assert.That(handler, Is.Not.Null);
    }

    [Test]
    public void QuestBoardDependencyInjection_ShouldRegisterHandlersFromAssembly()
    {
        var services = new ServiceCollection();
        services.AddQuestBoard(config => config.AddHandlersFromAssembly(typeof(TestHandler).Assembly));
        using var scope = services.BuildServiceProvider().CreateScope();
        var handler1 = scope.ServiceProvider.GetService<IHandler<TestRequest, TestResponse>>();
        var handler2 = scope.ServiceProvider.GetService<IHandlerAsync<TestAsyncRequest, TestResponse>>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(handler1, Is.Not.Null);
            Assert.That(handler2, Is.Not.Null);
        }
    }

    [Test]
    public void QuestBoardDependencyInjection_ShouldRegisterHandlersFromAssemblies()
    {
        var services = new ServiceCollection();
        services.AddQuestBoard(config => config.AddHandlersFromAsseblies([typeof(TestHandler).Assembly]));
        using var scope = services.BuildServiceProvider().CreateScope();
        var handler1 = scope.ServiceProvider.GetService<IHandler<TestRequest, TestResponse>>();
        var handler2 = scope.ServiceProvider.GetService<IHandlerAsync<TestAsyncRequest, TestResponse>>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(handler1, Is.Not.Null);
            Assert.That(handler2, Is.Not.Null);
        }
    }
}
