using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Common.Interfaces.Services;

namespace TodoApp.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
	private readonly IDateTimeProvider _dateTimeProvider;

	public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IDateTimeProvider dateTimeProvider)
	{
		_logger = logger;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<TResponse> Handle(
		TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Starting {@RequestName}, {@DateTimeUTC}",
								typeof(TRequest).Name,
								_dateTimeProvider.UtcNow);
		
		var result = await next();

		_logger.LogInformation("Completed {@RequestName}, {@DateTimeUTC}",
			typeof(TRequest).Name,
			_dateTimeProvider.UtcNow);

		return result;
	}
}