namespace TodoApp.TodoApi.Common.Contracts.Todos;

public record UpdateTodoRequest(
		string Title,
		string? Description,
		DateTime? DueDate,
		bool IsComplete
	);