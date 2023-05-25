namespace TodoApp.TodoApi.Common.Contracts.Authentication;

public record SignUpRequest(
		string FirstName,
		string LastName,
		string Email,
		string Password
	);