namespace FinTrack.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMediatR(opt => opt.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        
        return services;
    }
}