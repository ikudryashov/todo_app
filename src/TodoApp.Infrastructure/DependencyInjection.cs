using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Common.Interfaces.Services;
using TodoApp.Infrastructure.Authentication;
using TodoApp.Infrastructure.Persistence;
using TodoApp.Infrastructure.Services;

namespace TodoApp.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
	{
		services.Configure<JwtTokenOptions>(configuration.GetSection(JwtTokenOptions.ConfigSectionName));
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
		return services;
	}
}