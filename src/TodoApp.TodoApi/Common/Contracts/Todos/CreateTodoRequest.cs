namespace TodoApp.TodoApi.Common.Contracts.Todos;

public record CreateTodoRequest(
		string Title,
		string? Description,
		DateTime? DueDate
	);