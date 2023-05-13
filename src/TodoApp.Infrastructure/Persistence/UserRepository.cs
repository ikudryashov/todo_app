using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
	private static readonly List<User> Users = new();
	public User? GetUserByEmail(string email)
	{
		return Users.SingleOrDefault(user => user.Email == email);
	}

	public void CreateUser(User user)
	{
		Users.Add(user);
	}
}