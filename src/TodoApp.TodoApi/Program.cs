using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
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
	
	builder.Services.AddAuthorization(options => 
	{
		options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
	});
	
	builder.Services.AddAuthentication("Bearer")
		.AddJwtBearer(options => 
		{
			options.TokenValidationParameters = new()
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				ValidIssuer = builder.Configuration.GetValue<string>("JwtTokenOptions:Issuer"),
				ValidAudience = builder.Configuration.GetValue<string>("JwtTokenOptions:Audience"),
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtTokenOptions:SecretKey")!))
			};
		});
}

var app = builder.Build();

{
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}
	
	
	app.UseMiddleware<ErrorHandlingMiddleware>();
	app.UseAuthentication();
	app.UseHttpsRedirection();
	app.UseAuthorization();
	app.MapControllers();
}

app.Run();