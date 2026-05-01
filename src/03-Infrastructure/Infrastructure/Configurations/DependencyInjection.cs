using System.Data;
using System.Text;
using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Infrastructure.Identity;
using Infrastructure.Persistance.Repositories.Auth;
using Infrastructure.Persistance.Repositories.Catalog;
using Infrastructure.Persistance.Repositories.Finance;
using Infrastructure.Persistance.UnitOfWork;
using Infrastructure.Security;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddHttpContextAccessor();
        
        services.Configure<JwtSettings>(configuration.GetSection("JWT"));
        var jwtSettings = configuration.GetSection("JWT").Get<JwtSettings>();


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });
        
        services.AddScoped<IDbConnection>(_ =>
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        
        return services;
    }
}