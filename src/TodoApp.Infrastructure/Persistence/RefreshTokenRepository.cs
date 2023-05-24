using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence;

public class RefreshTokenRepository : IRefreshTokenRepository
{
	private readonly string _connectionString;

	public RefreshTokenRepository(IConfiguration configuration)
	{
		_connectionString = configuration.GetSection("Database")["ConnectionString"]!;
	}
	
	public async Task<RefreshToken?> GetRefreshTokenByUserId(Guid userId)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		var token = (await connection.QueryAsync<RefreshToken>
			(@"SELECT
			user_id as UserId,
			token as Token,
			salt as Salt,
			expiry_date as ExpiryDate,
			is_valid as IsValid
			FROM refresh_tokens WHERE user_id=@UserId", new { UserId = userId }))
			.AsList()
			.FirstOrDefault();
		return token;
	}

	public async Task CreateRefreshToken(RefreshToken refreshToken)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"INSERT INTO refresh_tokens 
    			(user_id, 
				token, 
                salt,
				expiry_date, 
				is_valid)
				VALUES (
				@UserId,
				@Token,
				@Salt,
				@ExpiryDate,
				@IsValid)", refreshToken);
	}

	public async Task UpdateRefreshToken(RefreshToken refreshToken)
	{
		await using var connection = new NpgsqlConnection(_connectionString);
		await connection.ExecuteAsync(
			@"UPDATE refresh_tokens SET
				token = @Token,
				salt = @Salt,
				expiry_date = @ExpiryDate,
				is_valid = @IsValid
				WHERE user_id = @UserId", 
			new
			{
				Token = refreshToken.Token,
				Salt = refreshToken.Salt,
				ExpiryDate = refreshToken.ExpiryDate,
				IsValid = refreshToken.IsValid,
				UserId = refreshToken.UserId
			});
	}
}