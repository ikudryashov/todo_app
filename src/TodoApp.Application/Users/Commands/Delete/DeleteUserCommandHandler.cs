using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Users.Commands.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
	private readonly IUserRepository _userRepository;

	public DeleteUserCommandHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(command.Id) is not User user)
		{
			throw new ApiException("Not found", "User does not exist", HttpStatusCode.NotFound);
		}

		await _userRepository.DeleteUser(command.Id);
	}
}