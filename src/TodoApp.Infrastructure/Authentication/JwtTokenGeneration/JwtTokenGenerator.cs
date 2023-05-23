using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Services;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Authentication.JwtTokenGeneration;

public class JwtTokenGenerator : IJwtTokenGenerator
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly JwtTokenOptions _jwtTokenOptions;

	public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtTokenOptions> jwtTokenOptions)
	{
		_dateTimeProvider = dateTimeProvider;
		_jwtTokenOptions = jwtTokenOptions.Value;
	}

	public string GenerateToken(User user)
	{
		var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(_jwtTokenOptions.SecretKey)),
				SecurityAlgorithms.HmacSha256
			);

		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
			new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		var token = new JwtSecurityToken(
				issuer: _jwtTokenOptions.Issuer,
				audience: _jwtTokenOptions.Audience,
				claims: claims,
				expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtTokenOptions.ExpiryMinutes),
				signingCredentials: signingCredentials
			);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public RefreshToken GenerateRefreshToken()
	{
		return new RefreshToken()
		{
			Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(60)),
			ExpiryDate = _dateTimeProvider.UtcNow.AddDays(_jwtTokenOptions.RefreshTokenExpiryDays)
		};
	}
}