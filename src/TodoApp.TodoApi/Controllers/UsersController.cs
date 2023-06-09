using System.Security.Claims;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Users.Commands.Delete;
using TodoApp.Application.Users.Commands.Update;
using TodoApp.Application.Users.Queries;
using TodoApp.TodoApi.Common.Contracts.Users;

namespace TodoApp.TodoApi.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public UsersController(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	[HttpGet("/api/users/{id}")]
	public async Task<IActionResult> GetUser(Guid id)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		
		var query = new GetUserQuery(id, userId);
		var result = await _mediator.Send(query);
		var response = _mapper.Map<UserResponse>(result);
		
		return Ok(response);
	}
	
	[HttpPut("/api/users/{id}")]
	public async Task<IActionResult> UpdateUser(Guid id, [FromBody]UpdateUserRequest request)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		
		var command = (id, userId, request).Adapt<UpdateUserCommand>();
		await _mediator.Send(command);
		
		return NoContent();
	}

	[HttpDelete("/api/users/{id}")]
	public async Task<IActionResult> DeleteUser(Guid id)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		
		var command = new DeleteUserCommand(id, userId);
		await _mediator.Send(command);
		
		return NoContent();
	}
	private Guid GetUserId(HttpContext context)
	{
		var subClaim = context.User.Claims
			.FirstOrDefault(claim => 
				claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));
		return Guid.Parse(subClaim!.Value);
	}
}