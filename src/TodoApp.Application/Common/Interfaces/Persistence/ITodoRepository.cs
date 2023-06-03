using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Interfaces.Persistence;

public interface ITodoRepository
{
	public Task<List<Todo?>> GetTodos(Guid userId);
	public Task<Todo?> GetTodoById(Guid todoId);
	public Task CreateTodo(Todo todo);
	public Task UpdateTodo(Todo todo);
	public Task DeleteTodo(Guid todoId);
}