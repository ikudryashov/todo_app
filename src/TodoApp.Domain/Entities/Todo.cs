namespace TodoApp.Domain.Entities;

public class Todo
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public string Title { get; set; } = null!;
	public string? Description { get; set; }
	public DateTime? DueDate { get; set; }
}