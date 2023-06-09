using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace TodoApp.TodoApi.Health;

public class DatabaseHealthCheck : IHealthCheck
{
	private readonly IConfiguration _configuration;

	public DatabaseHealthCheck(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task<HealthCheckResult> CheckHealthAsync(
		HealthCheckContext context,
		CancellationToken cancellationToken = new ())
	{
		try
		{
			await using var connection = new NpgsqlConnection(
				_configuration.GetSection("Database")["ConnectionString"]!);
			
			await connection.ExecuteScalarAsync("SELECT 1");
			
			return HealthCheckResult.Healthy();
		}
		catch (Exception ex)
		{
			return HealthCheckResult.Unhealthy(exception: ex);
		}
	}
}