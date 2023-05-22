using TodoApp.Application;
using TodoApp.Infrastructure;
using TodoApp.TodoApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
{
	builder.Services.AddControllers();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();
	builder.Services.AddApplication();
	builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();

{
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseMiddleware<ErrorHandlingMiddleware>();
	app.UseHttpsRedirection();
	app.UseAuthorization();
	app.MapControllers();
}

app.Run();