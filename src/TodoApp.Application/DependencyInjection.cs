using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Services.Authentication;
using TodoApp.Application.Services.Authentication.Commands;
using TodoApp.Application.Services.Authentication.Queries;

namespace TodoApp.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();
		services.AddScoped<IAuthenticationQueryService, AuthenticationQueryService>();
		return services;
	}
}
