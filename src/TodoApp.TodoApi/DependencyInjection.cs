using System.Text;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TodoApp.TodoApi.Health;

namespace TodoApp.TodoApi;


public static class DependencyInjection
{
	public static IServiceCollection AddApi(this IServiceCollection services, ConfigurationManager configuration, 
		ConfigureHostBuilder host)
	{
		services.AddAuthorization(options => 
		{
			options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
		});
	
		services.AddAuthentication("Bearer")
			.AddJwtBearer(options => 
			{
				options.TokenValidationParameters = new()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ValidIssuer = configuration.GetValue<string>("JwtTokenOptions:Issuer"),
					ValidAudience = configuration.GetValue<string>("JwtTokenOptions:Audience"),
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtTokenOptions:SecretKey")!))
				};
			});
		
		//logging
		host.UseSerilog((context, loggerConfiguration) =>
		{
			loggerConfiguration.ReadFrom.Configuration(context.Configuration);
		});
	
		//rate limiting
		services.AddMemoryCache();

		services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
		services.AddInMemoryRateLimiting();
		services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

		services.AddHealthChecks()
			.AddCheck<DatabaseHealthCheck>("Database");
		
		return services;
	}
}