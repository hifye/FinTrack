using System.Data;
using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Infrastructure.Persistance.Repositories.Auth;
using Infrastructure.Persistance.Repositories.Catalog;
using Infrastructure.Persistance.Repositories.Finance;
using Infrastructure.Persistance.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
        
        services.AddScoped<IDbConnection>(_ =>
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}