using Mapster;
using TodoApp.Application.Users.Commands.Update;
using TodoApp.Application.Users.Common;
using TodoApp.TodoApi.Common.Contracts.Users;

namespace TodoApp.TodoApi.Common.Mapping;

public class UsersMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<(Guid Id, Guid userId, UpdateUserRequest request), UpdateUserCommand>()
			.Map(dest => dest.Id, src => src.Id)
			.Map(dest => dest.RequestId, src => src.userId)
			.Map(dest => dest.FirstName, src => src.request.FirstName)
			.Map(dest => dest.LastName, src => src.request.LastName)
			.Map(dest => dest.Email, src => src.request.Email);
	}
}