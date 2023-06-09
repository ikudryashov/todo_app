using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Users.Commands.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
	private readonly IUserRepository _userRepository;

	public DeleteUserCommandHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
	{
		if (command.Id != command.RequestId)
		{
			throw new ApiException("Unauthorized", "You do not have access to this resource.", 
				nameof(DeleteUserCommand),
				HttpStatusCode.Unauthorized);
		}
		
		if (await _userRepository.GetUserById(command.Id) is not User user)
		{
			throw new ApiException("Not found", "User does not exist", nameof(DeleteUserCommand), 
				HttpStatusCode.NotFound);
		}

		await _userRepository.DeleteUser(command.Id);
		
		return Unit.Value;
	}
}