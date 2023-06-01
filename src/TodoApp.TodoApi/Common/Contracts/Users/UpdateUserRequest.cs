namespace TodoApp.TodoApi.Common.Contracts.Users;

public record UpdateUserRequest(
		string FirstName,
		string LastName,
		string Email
);