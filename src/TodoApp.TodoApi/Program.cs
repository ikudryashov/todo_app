using TodoApp.Application;
using TodoApp.Infrastructure;
using TodoApp.TodoApi;
using TodoApp.TodoApi.Common.Mapping;
using TodoApp.TodoApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
{
	builder.Services.AddControllers();
	builder.Services.AddMappings();
	builder.Services.AddApplication();
	builder.Services.AddInfrastructure(builder.Configuration);
	builder.Services.AddApi(builder.Configuration);
}

var app = builder.Build();

{
	app.UseAuthentication();
	app.UseHttpsRedirection();
	app.UseAuthorization();
	app.MapControllers();
	app.UseMiddleware<ErrorHandlingMiddleware>();
}

app.Run();