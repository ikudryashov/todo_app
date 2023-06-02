using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Users.Commands.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
	private readonly IUserRepository _userRepository;

	public UpdateUserCommandHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository; ;
	}

	public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(command.Id) is not User user)
		{
			throw new ApiException("Not found", "User does not exist.", HttpStatusCode.NotFound);
		}

		var updatedUser = new User
		{
			Id = command.Id,
			FirstName =  command.FirstName,
			LastName = command.LastName,
			Email = command.Email,
			Password =  user.Password,
			Salt = user.Salt
		};
		
		await _userRepository.UpdateUser(updatedUser);

		return Unit.Value;
	}
}