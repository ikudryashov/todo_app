using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services.Authentication;

public record AuthenticationResult(
		User User,
		string Token
	);