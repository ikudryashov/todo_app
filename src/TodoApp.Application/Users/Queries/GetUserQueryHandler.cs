using System.Net;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Users.Common;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Users.Queries;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserResult>
{
	private readonly IUserRepository _userRepository;

	public GetUserQueryHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<UserResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
	{
		if (await _userRepository.GetUserById(request.Id) is not User user)
		{
			throw new ApiException("Not found", "User does not exist", HttpStatusCode.NotFound);
		}
		
		return new UserResult(user.Id, user.FirstName, user.LastName, user.Email);
	}
}