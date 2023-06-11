using System.Net;
using System.Security.Claims;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Authentication.Commands.LogOut;
using TodoApp.Application.Authentication.Commands.Refresh;
using TodoApp.Application.Authentication.Commands.SignUp;
using TodoApp.Application.Authentication.Queries.LogIn;
using TodoApp.Application.Common.Exceptions;
using TodoApp.TodoApi.Common.Contracts.Authentication;

namespace TodoApp.TodoApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public AuthenticationController(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	[HttpPost("api/auth/signup")]
	[AllowAnonymous]
	public async Task<IActionResult> SignUp(SignUpRequest request)
	{
		var command = _mapper.Map<SignUpCommand>(request);

		var authResult = await _mediator.Send(command);
		var response = _mapper.Map<AuthResponse>(authResult);

		return Ok(response);
	}
	
	[HttpPost("api/auth/login")]
	[AllowAnonymous]
	public async Task<IActionResult> LogIn(LogInRequest request)
	{
		var query = _mapper.Map<LogInQuery>(request);
		
		var authResult = await _mediator.Send(query);
		var response = _mapper.Map<AuthResponse>(authResult);
		
		return Ok(response);
	}

	[HttpPost("api/auth/refresh")]
	[AllowAnonymous]
	public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
	{
		var context = HttpContext;
		var userId = GetUserId(context);

		var command = (userId, request).Adapt<RefreshCommand>();
		var authResult = await _mediator.Send(command);
		var response = _mapper.Map<AuthResponse>(authResult);
		
		return Ok(response);
	}

	[HttpPost("api/auth/logout")]
	public async Task<IActionResult> LogOut()
	{
		var context = HttpContext;
		var userId = GetUserId(context);

		var command = new LogOutCommand(userId);
		await _mediator.Send(command);

		return NoContent();
	}
	
	
	private Guid GetUserId(HttpContext context)
	{
		var subClaim = context.User.Claims.FirstOrDefault(
			claim => claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));
		return Guid.Parse(subClaim!.Value);
	}
}