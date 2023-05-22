using TodoApp.Application.Common.Exceptions.Authentication;
using TodoApp.Application.Common.Interfaces.Authentication;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Services.Authentication.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Services.Authentication.Commands;

public class AuthenticationCommandService : IAuthenticationCommandService
{
	private readonly IJwtTokenGenerator _jwtTokenGenerator;
	private readonly IUserRepository _userRepository;

	public AuthenticationCommandService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
	}

	public AuthenticationResult SignUpCommand(string firstName, string lastName, string email, string password)
	{
		if (_userRepository.GetUserByEmail(email) is not null)
		{
			throw new DuplicateEmailException();
		}

		//create user
		var user = new User()
		{
			Id = Guid.NewGuid(),
			FirstName = firstName,
			LastName = lastName,
			Email = email,
			Password = password
		};
		
		//persist user
		_userRepository.CreateUser(user);
		
		//generate token
		var token = _jwtTokenGenerator.GenerateToken(user);
		
		return new AuthenticationResult(user, token);
	}
}