using TodoApp.Application.Common.Interfaces.Services;

namespace TodoApp.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;
}