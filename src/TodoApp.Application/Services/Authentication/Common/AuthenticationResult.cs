using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services.Authentication.Common;

public record AuthenticationResult(
		User User,
		string Token
	);