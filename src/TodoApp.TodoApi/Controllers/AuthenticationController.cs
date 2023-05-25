using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Authentication.Commands.LogOut;
using TodoApp.Application.Authentication.Commands.Refresh;
using TodoApp.Application.Authentication.Commands.SignUp;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Authentication.Queries.LogIn;
using TodoApp.Domain.Exceptions.User.Authentication;
using TodoApp.TodoApi.Contracts.Authentication;

namespace TodoApp.TodoApi.Controllers;

[ApiController]
[AllowAnonymous]
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

		return Ok(authResult);
	}
	
	[HttpPost("/auth/login")]
	public async Task<IActionResult> LogIn(LogInRequest request)
	{
		var query = new LogInQuery(request.Email, request.Password);
		var authResult = await _mediator.Send(query);

		return Ok(authResult);
	}

	[HttpPost("/auth/refresh")]
	public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
	{
		var context = HttpContext;
		var claimedId = GetUserId(context);

		var command = new RefreshCommand(Guid.Parse(claimedId), request.RefreshToken);
		
		var authResult = await _mediator.Send(command);
		
		return Ok(authResult);
	}

	[HttpPost("/auth/logout")]
	public async Task<IActionResult> LogOut([FromBody] LogOutRequest request)
	{
		var context = HttpContext;
		var claimedId = GetUserId(context);

		var command = new LogOutCommand(Guid.Parse(claimedId), request.RefreshToken);
		await _mediator.Send(command);

		return NoContent();
	}
	
	
	private string GetUserId(HttpContext context)
	{
		var subClaim = context.User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));
		if (subClaim?.Value is null) throw new InvalidCredentialsException();
		return subClaim.Value;
	}
}