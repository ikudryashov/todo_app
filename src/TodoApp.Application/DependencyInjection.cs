using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Services.Authentication;

namespace TodoApp.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IAuthenticationService, AuthenticationService>();
		return services;
	}
}