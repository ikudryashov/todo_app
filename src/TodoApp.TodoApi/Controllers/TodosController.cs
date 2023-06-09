using System.Security.Claims;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Todos.Commands.Create;
using TodoApp.Application.Todos.Commands.Delete;
using TodoApp.Application.Todos.Commands.Update;
using TodoApp.Application.Todos.Queries.GetTodoById;
using TodoApp.Application.Todos.Queries.GetTodos;
using TodoApp.TodoApi.Common.Contracts.Todos;

namespace TodoApp.TodoApi.Controllers;

[ApiController]
public class TodosController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;

	public TodosController(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}

	[HttpGet("api/todos/")]
	public async Task<IActionResult> GetTodos()
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		var query = new GetTodosQuery(userId);
		var result = await _mediator.Send(query);
		var response = _mapper.Map<List<TodoResponse>>(result);

		return Ok(response);
	}
	
	[HttpGet("api/todos/{todoId:guid}")]
	public async Task<IActionResult> GetTodoById(Guid todoId)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		var query = new GetTodoByIdQuery(todoId, userId);
		var result = await _mediator.Send(query);
		var response = _mapper.Map<TodoResponse>(result);

		return Ok(response);
	}
	
	[HttpPost("api/todos/")]
	public async Task<IActionResult> CreateTodo(CreateTodoRequest request)
	{
		var context = HttpContext;
		var userId = GetUserId(context);
		var command = (userId, request).Adapt<CreateTodoCommand>();
		var result = await _mediator.Send(command);
		var response = _mapper.Map<TodoResponse>(result);
		
		return Created(nameof(GetTodoById), response);
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
		var subClaim = context.User.Claims
			.FirstOrDefault(claim => 
				claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase));
		return Guid.Parse(subClaim!.Value);
	}
}