using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Services;

namespace TodoApp.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly JwtTokenOptions _jwtTokenOptions;

	public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtTokenOptions> jwtTokenOptions)
	{
		_dateTimeProvider = dateTimeProvider;
		_jwtTokenOptions = jwtTokenOptions.Value;
	}

	public string GenerateToken(Guid id, string firstName, string lastName)
	{
		var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(_jwtTokenOptions.SecretKey)),
				SecurityAlgorithms.HmacSha256
			);

		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
			new Claim(JwtRegisteredClaimNames.GivenName, firstName),
			new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
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
}