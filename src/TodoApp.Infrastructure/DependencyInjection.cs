using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Common.Interfaces.Services;
using TodoApp.Infrastructure.Authentication;
using TodoApp.Infrastructure.Authentication.CredentialsHashing;
using TodoApp.Infrastructure.Authentication.JwtTokenGeneration;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Services;

namespace TodoApp.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
	{
		services.Configure<JwtTokenOptions>(configuration.GetSection(JwtTokenOptions.ConfigSectionName));
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
		services.AddScoped<ITodoRepository, TodoRepository>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
		services.AddSingleton<ICredentialsHasher, CredentialsHasher>();
		return services;
	}
}