using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
	Task<User?> GetUserByEmail(string email);
	Task<User?> GetUserById(Guid id);
	Task CreateUser(User user);
	Task UpdateUser(User user);
	
}