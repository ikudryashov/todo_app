using System.Net;
using System.Security.Claims;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common.Exceptions;
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
		var context = HttpContext;
		if (!ValidateUserId(id, context))
			throw new ApiException("Unauthorized", "You do not have access to this resource",
				HttpStatusCode.Unauthorized);
		
		var query = new GetUserQuery(id);
		var response = await _mediator.Send(query);
		
		return Ok(response);
	}



	[HttpPut("/api/users/{id}")]
	public async Task<IActionResult> UpdateUser(Guid id, [FromBody]UpdateUserRequest request)
	{
		var context = HttpContext;
		if (!ValidateUserId(id, context))
			throw new ApiException("Unauthorized", "You do not have access to this resource",
				HttpStatusCode.Unauthorized);
		
		var command = (id, request).Adapt<UpdateUserCommand>();
		await _mediator.Send(command);
		
		return NoContent();
	}

	[HttpDelete("/api/users/{id}")]
	public async Task<IActionResult> DeleteUser(Guid id)
	{
		var context = HttpContext;
		if (!ValidateUserId(id, context))
			throw new ApiException("Unauthorized", "You do not have access to this resource",
				HttpStatusCode.Unauthorized);
		
		var command = new DeleteUserCommand(id);
		await _mediator.Send(command);
		
		return NoContent();
	}
	private bool ValidateUserId(Guid id, HttpContext context)
    {
	    var subClaim = context.User.Claims.FirstOrDefault(
		    claim => claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));

	    return (string.Equals(id.ToString(), subClaim!.Value));
    }
}