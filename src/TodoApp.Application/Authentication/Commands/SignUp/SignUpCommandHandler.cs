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
	private readonly IPasswordHasher _passwordHasher;
	private readonly IUserRepository _userRepository;

	public SignUpCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
		_passwordHasher = passwordHasher;
	}

	public async Task<AuthenticationResult> Handle(SignUpCommand command, CancellationToken cancellationToken)
	{
		if (_userRepository.GetUserByEmail(command.Email) is not null)
		{
			throw new DuplicateEmailException();
		}

		//create user
		var hashResult = _passwordHasher.HashPassword(command.Password);
		var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
		
		var user = new User()
		{
			Id = Guid.NewGuid(),
			FirstName = command.FirstName,
			LastName = command.LastName,
			Email = command.Email,
			Password = hashResult.hash,
			Salt =  hashResult.salt,
			RefreshToken = refreshToken.Token,
			RefreshTokenExpiryDate = refreshToken.ExpiryDate
		};
		
		//persist user
		_userRepository.CreateUser(user);
		
		//generate jwt token
		var jwtToken = _jwtTokenGenerator.GenerateToken(user);
		
		return new AuthenticationResult(user, jwtToken);
	}
}