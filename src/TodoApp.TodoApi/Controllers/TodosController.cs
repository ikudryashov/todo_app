using System.Net;
using System.Security.Claims;
using MapsterMapper;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Todos.Commands;
using TodoApp.Application.Todos.Commands.Create;
using TodoApp.Application.Todos.Commands.Delete;
using TodoApp.Application.Todos.Commands.Update;
using TodoApp.Application.Todos.Queries;
using TodoApp.Application.Todos.Queries.GetTodoById;
using TodoApp.Application.Todos.Queries.GetTodos;
using TodoApp.TodoApi.Common.Contracts.Todos;

namespace TodoApp.TodoApi.Controllers;

[ApiController]
public class TodosController : ControllerBase
{
	private readonly IMediator _mediator;

	public TodosController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/todos/")]
	public async Task<IActionResult> GetTodos()
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		var query = new GetTodosQuery(userId);
		var response = await _mediator.Send(query);

		return Ok(response);
	}
	
	[HttpGet("api/todos/{todoId:guid}")]
	public async Task<IActionResult> GetTodoById(Guid todoId)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		var query = new GetTodoByIdQuery(todoId, userId);
		var response = await _mediator.Send(query);

		return Ok(response);
	}
	
	[HttpPost("api/todos/")]
	public async Task<IActionResult> CreateTodo(CreateTodoRequest request)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		var command = (userId, request).Adapt<CreateTodoCommand>();
		var response = await _mediator.Send(command);

		return Ok(response);
	}
	
	[HttpPut("api/todos/{todoId:guid}")]
	public async Task<IActionResult> UpdateTodo(Guid todoId, UpdateTodoRequest request)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		var command = (todoId, userId, request).Adapt<UpdateTodoCommand>();
		await _mediator.Send(command);

		return NoContent();
	}
	
	[HttpDelete("api/todos/{todoId:guid}")]
	public async Task<IActionResult> DeleteTodo(Guid todoId)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		var command = (todoId, userId).Adapt<DeleteTodoCommand>();
		await _mediator.Send(command);

		return NoContent();
	}

	private Guid GetUserId(HttpContext context)
	{
		var subClaim = context.User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));
		if (subClaim?.Value is null) throw new ApiException(
			"Failed to authenticate",
			"Invalid access credentials.", 
			HttpStatusCode.Unauthorized);
		return Guid.Parse(subClaim.Value);
	}
	
}