using BumaSoft.QuestBoard.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BumaSoft.QuestBoard.DependencyInjection;

public static class QuestBoardDependencyInjection
{
    public static IServiceCollection AddQuestBoard(this IServiceCollection services, Action<QuestBoardConfiguration>? configure = null)
    {
        var configuration = new QuestBoardConfiguration(services);
        if (configure is not null)
            configure(configuration);
        var busType = configuration.BusType;
        services.AddScoped(busType);
        services.AddScoped(typeof(IBus), serviceProvider => serviceProvider.GetRequiredService(busType));
        return services;
    }
}
