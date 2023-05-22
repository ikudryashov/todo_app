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
	private readonly IUserRepository _userRepository;

	public SignUpCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
	{
		_jwtTokenGenerator = jwtTokenGenerator;
		_userRepository = userRepository;
	}

	public async Task<AuthenticationResult> Handle(SignUpCommand command, CancellationToken cancellationToken)
	{
		if (_userRepository.GetUserByEmail(command.Email) is not null)
		{
			throw new DuplicateEmailException();
		}

		//create user
		var user = new User()
		{
			Id = Guid.NewGuid(),
			FirstName = command.FirstName,
			LastName = command.LastName,
			Email = command.Email,
			Password = command.Password
		};
		
		//persist user
		_userRepository.CreateUser(user);
		
		//generate token
		var token = _jwtTokenGenerator.GenerateToken(user);
		
		return new AuthenticationResult(user, token);
	}
}