using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence;

public class TodoRepository : ITodoRepository
{
	private readonly string _connectionString;

	public TodoRepository(IConfiguration configuration)
	{
		_connectionString = configuration.GetSection("Database")["ConnectionString"]!;
	}
	public async Task<List<Todo?>> GetTodos(Guid userId)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		var todos = (await connection.QueryAsync<Todo>(
			@"SELECT id as Id,
				  user_id as UserId,
				  title as Title,
				  description as Description,
				  due_date as DueDate,
				  is_complete as IsComplete
				  FROM todos WHERE user_id=@Id", new { Id = userId })).AsList();

		return todos!;
	}

	public async Task<Todo?> GetTodoById(Guid todoId)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		var todo = (await connection.QueryAsync<Todo>(
			@"SELECT id as Id,
				  user_id as UserId,
				  title as Title,
				  description as Description,
				  due_date as DueDate,
				  is_complete as IsComplete
				  FROM todos WHERE id=@TodoId", new { TodoId = todoId })).AsList().FirstOrDefault();

		return todo;
	}

	public async Task CreateTodo(Todo todo)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"INSERT INTO todos
				(id, user_id, title, description, due_date, is_complete)
    			VALUES
    			(@Id, @UserId, @Title, @Description, @DueDate, @IsComplete)", todo);
	}

	public async Task UpdateTodo(Todo todo)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"UPDATE todos SET
				title = @Title, description = @Description, due_date = @DueDate, is_complete = @IsComplete
             	WHERE id = @Id", todo);
	}

	public async Task DeleteTodo(Guid todoId)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"DELETE FROM todos WHERE id=@Id", new { Id = todoId });
	}
}