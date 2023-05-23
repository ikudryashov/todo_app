using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Authentication.Commands.Refresh;
using TodoApp.Application.Authentication.Commands.SignUp;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Authentication.Queries.LogIn;
using TodoApp.TodoApi.Contracts;
using TodoApp.TodoApi.Contracts.Authentication;

namespace TodoApp.TodoApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly IMediator _mediator;

	public AuthenticationController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("/auth/signup")]
	public async Task<IActionResult> SignUp(SignUpRequest request)
	{
		var command = new SignUpCommand(
			request.FirstName,
			request.LastName,
			request.Email,
			request.Password);

		var authResult = await _mediator.Send(command);

		var response = MapAuthenticationResult(authResult);
		
		return Ok(response);
	}
	
	[HttpPost("/auth/login")]
	public async Task<IActionResult> LogIn(LogInRequest request)
	{
		var query = new LogInQuery(request.Email, request.Password);
		var authResult = await _mediator.Send(query);

		var response = MapAuthenticationResult(authResult);
		
		return Ok(response);
	}

	[HttpPost("/auth/refresh")]
	public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
	{
		var command = new RefreshCommand(request.RefreshToken);
		var authResult = await _mediator.Send(command);
		
		var response = MapAuthenticationResult(authResult);
		return Ok(response);
	}

	private AuthResponse MapAuthenticationResult(AuthenticationResult result)
	{
		return new AuthResponse(
			result.User.Id,
			result.User.FirstName,
			result.User.LastName,
			result.User.Email,
			result.Token,
			result.User.RefreshToken
		);
	}
	
}