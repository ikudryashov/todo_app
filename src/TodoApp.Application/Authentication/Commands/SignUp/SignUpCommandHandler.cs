using MediatR;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Exceptions.User.Authentication;

namespace TodoApp.Application.Authentication.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthenticationResult>
{
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly ICredentialsHasher _credentialsHasher;
	private readonly IUserRepository _userRepository;
	private readonly IRefreshTokenRepository _refreshTokenRepository;

	public SignUpCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, ICredentialsHasher credentialsHasher, IRefreshTokenRepository refreshTokenRepository)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
		_credentialsHasher = credentialsHasher;
		_refreshTokenRepository = refreshTokenRepository;
	}

	public async Task<AuthenticationResult> Handle(SignUpCommand command, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserByEmail(command.Email) is not null)
		{
			throw new DuplicateEmailException();
		}

		//create user
		var passwordHash = _credentialsHasher.Hash(command.Password);
		
		var user = new User()
		{
			Id = Guid.NewGuid(),
			FirstName = command.FirstName,
			LastName = command.LastName,
			Email = command.Email,
			Password = passwordHash.hash,
			Salt =  passwordHash.salt,
		};
		
		// generate tokens
		var accessToken = _jwtTokenGenerator.GenerateToken(user);
		var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);

		// form result
		var result = new AuthenticationResult(user.Id, accessToken, refreshToken.Token);
		
		//persist user and their refresh token
		var hashedRefreshToken = _credentialsHasher.Hash(refreshToken.Token);
		refreshToken.Token = hashedRefreshToken.hash;
		refreshToken.Salt = hashedRefreshToken.salt;
		await _userRepository.CreateUser(user);
		await _refreshTokenRepository.CreateRefreshToken(refreshToken);

		return result;
	}
}