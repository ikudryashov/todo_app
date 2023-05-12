using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Services.Authentication;
using TodoApp.TodoApi.Contracts;
using TodoApp.TodoApi.Contracts.Authentication;

namespace TodoApp.TodoApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly IAuthenticationService _authenticationService;

	public AuthenticationController(IAuthenticationService authenticationService)
	{
		_authenticationService = authenticationService;
	}

	[HttpPost("/auth/signup")]
	public IActionResult SignUp(SignUpRequest request)
	{
		var authResult =
			_authenticationService.SignUp(
				request.FirstName, 
				request.LastName, 
				request.Email, 
				request.Password);

		var response = new AuthResponse(
				authResult.Id,
				authResult.FirstName,
				authResult.LastName,
				authResult.Email,
				authResult.Token
			);
		
		return Ok(response);
	}
	
	[HttpPost("/auth/login")]
	public IActionResult LogIn(LogInRequest request)
	{
		var authResult =
			_authenticationService.LogIn(
				request.Email, 
				request.Password);

		var response = new AuthResponse(
			authResult.Id,
			authResult.FirstName,
			authResult.LastName,
			authResult.Email,
			authResult.Token
		);
		
		return Ok(response);
	}
}