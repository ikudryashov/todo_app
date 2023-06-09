namespace TodoApp.Application.Todos.Common;

public record TodoResult(
		Guid Id,
		Guid UserId,
		string Title,
		string? Description,
		DateTime? DueDate,
		bool IsComplete
	);