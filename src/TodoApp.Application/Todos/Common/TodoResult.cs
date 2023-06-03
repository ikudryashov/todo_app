namespace TodoApp.Application.Todos.Common;

public record TodoResult(
		Guid Id,
		string Title,
		string? Description,
		DateTime? DueDate,
		bool IsComplete
	);