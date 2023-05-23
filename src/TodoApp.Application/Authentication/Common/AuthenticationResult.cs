using TodoApp.Domain.Entities;

namespace TodoApp.Application.Authentication.Common;

public record AuthenticationResult(
		User User,
		string Token
);