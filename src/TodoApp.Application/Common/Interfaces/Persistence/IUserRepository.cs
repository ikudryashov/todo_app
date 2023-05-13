using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
	User? GetUserByEmail(string email);
	void CreateUser(User user);
}