using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Common.Behaviors;

namespace TodoApp.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		return services;
	}
}
