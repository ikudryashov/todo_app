using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
	User? GetUserByEmail(string email);
	User? GetUserByRefreshToken(string refreshToken);
	void CreateUser(User user);
	void UpdateUser(User user);
	
}