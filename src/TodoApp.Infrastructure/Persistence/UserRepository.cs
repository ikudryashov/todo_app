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

	public User? GetUserByRefreshToken(string refreshToken)
	{
		return Users.SingleOrDefault(user => user.RefreshToken == refreshToken);
	}

	public void CreateUser(User user)
	{
		Users.Add(user);
	}

	public void UpdateUser(User user)
	{
		var prevUser = Users.SingleOrDefault(prev => prev.Email == user.Email);
		if (prevUser is not null)
		{
			prevUser.Id = user.Id;
			prevUser.FirstName = user.FirstName;
			prevUser.LastName = user.LastName;
			prevUser.Email = user.Email;
			prevUser.Password = user.Password;
			prevUser.Salt = user.Salt;
			prevUser.RefreshToken = user.RefreshToken;
			prevUser.RefreshTokenExpiryDate = user.RefreshTokenExpiryDate;
		}
	}
}