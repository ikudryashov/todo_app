using TodoApp.Domain.Entities;

namespace TodoApp.Application.Common.Interfaces.Persistence;

public interface IRefreshTokenRepository
{
	Task<RefreshToken?> GetRefreshTokenByUserId(Guid userId);
	Task CreateRefreshToken(RefreshToken refreshToken);
	Task UpdateRefreshToken(RefreshToken refreshToken);

}