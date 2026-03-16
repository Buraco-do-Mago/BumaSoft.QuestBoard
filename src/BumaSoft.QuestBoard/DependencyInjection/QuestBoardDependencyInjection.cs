using Microsoft.Extensions.DependencyInjection;

namespace BumaSoft.QuestBoard.DependencyInjection;

public static class QuestBoardDependencyInjection
{
    public static IServiceCollection AddQuestBoard(this IServiceCollection services, Action<QuestBoardConfiguration>? configure = null)
    {
        services.AddScoped<Bus>();
        if (configure is not null)
            configure(new QuestBoardConfiguration(services));
        return services;
    }
}
