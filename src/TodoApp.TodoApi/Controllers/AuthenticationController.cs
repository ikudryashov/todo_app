using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Services.Authentication.Commands;
using TodoApp.Application.Services.Authentication.Common;
using TodoApp.Application.Services.Authentication.Queries;
using TodoApp.TodoApi.Contracts;
using TodoApp.TodoApi.Contracts.Authentication;

namespace TodoApp.TodoApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly IAuthenticationCommandService _authenticationCommandService;
	private readonly IAuthenticationQueryService _authenticationQueryService;

	public AuthenticationController(
		IAuthenticationCommandService authenticationCommandService,
		IAuthenticationQueryService authenticationQueryService)
	{
		_authenticationCommandService = authenticationCommandService;
		_authenticationQueryService = authenticationQueryService;
	}

	[HttpPost("/auth/signup")]
	public IActionResult SignUp(SignUpRequest request)
	{
		var authResult =
			_authenticationCommandService.SignUpCommand(
				request.FirstName, 
				request.LastName, 
				request.Email, 
				request.Password);

		var response = MapAuthenticationResult(authResult);
		
		return Ok(response);
	}
	
	[HttpPost("/auth/login")]
	public IActionResult LogIn(LogInRequest request)
	{
		var authResult =
			_authenticationQueryService.LogInQuery(
				request.Email, 
				request.Password);

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
			result.Token
		);
	}
	
}