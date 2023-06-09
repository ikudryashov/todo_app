namespace TodoApp.TodoApi.Common.Contracts.Todos;

public record TodoResponse(
		string Id,
		string Title,
		string? Description,
		DateTime? DueDate,
		bool IsComplete
	);