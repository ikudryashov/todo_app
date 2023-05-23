using TodoApp.Application.Authentication.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
	public string GenerateToken(User user);
	public RefreshToken GenerateRefreshToken();
}