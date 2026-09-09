using Microsoft.Extensions.DependencyInjection;

namespace TermPaper.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
    
}