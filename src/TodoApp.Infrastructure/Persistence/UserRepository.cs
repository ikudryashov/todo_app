using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
	private readonly string _connectionString;

	public UserRepository(IConfiguration configuration)
	{
		_connectionString = configuration.GetSection("Database")["ConnectionString"]!;
	}

	public async Task<User?> GetUserByEmail(string email)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		var user = (await connection.QueryAsync<User>
		(@"SELECT
			id as Id,
			first_name as FirstName,
			last_name as LastName,
			email as Email,
			password as Password,
			salt as Salt
			FROM users WHERE email=@Email", new { Email = email }))
			.AsList()
			.FirstOrDefault();
		return user;
	}

	public async Task<User?> GetUserById(Guid id)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		var user = (await connection.QueryAsync<User>
			(@"SELECT
			id as Id,
			first_name as FirstName,
			last_name as LastName,
			email as Email,
			password as Password,
			salt as Salt
			FROM users WHERE id=@Id", new { Id = id }))
			.AsList()
			.FirstOrDefault();
		return user;
	}

	public async Task CreateUser(User user)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"INSERT INTO users (    
                id, 
				first_name, 
				last_name, 
				email, 
				password, 
				salt)
				VALUES (
				@Id,
				@FirstName,
				@LastName,
				@Email,
				@Password,
				@Salt)", user);
	}

	public async Task UpdateUser(User user)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"UPDATE users SET
				first_name = @FirstName,
				last_name = @LastName,
				email = @Email,
				password = @Password,
				salt = @Salt
				WHERE id = @Id", user);
	}
	
	public async Task DeleteUser(Guid id)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"DELETE FROM users
				WHERE id = @Id", new { Id = id });
	}
}