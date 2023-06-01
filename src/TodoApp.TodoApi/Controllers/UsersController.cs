using Mapster;
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

	public UsersController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("/api/users/{id}")]
	public async Task<IActionResult> GetUser(Guid id)
	{
		var query = new GetUserQuery(id);
		var response = await _mediator.Send(query);
		
		return Ok(response);
	}
	
	[HttpPut("/api/users/{id}")]
	public async Task<IActionResult> UpdateUser(Guid id, [FromBody]UpdateUserRequest request)
	{
		var command = (id, request).Adapt<UpdateUserCommand>();
		await _mediator.Send(command);
		
		return NoContent();
	}

	[HttpDelete("/api/users/{id}")]
	public async Task<IActionResult> DeleteUser(Guid id)
	{
		var command = new DeleteUserCommand(id);
		await _mediator.Send(command);
		
		return NoContent();
	}
	
}