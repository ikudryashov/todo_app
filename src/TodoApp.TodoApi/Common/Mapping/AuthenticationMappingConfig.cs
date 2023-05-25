using Mapster;
using TodoApp.Application.Authentication.Commands.LogOut;
using TodoApp.Application.Authentication.Commands.Refresh;
using TodoApp.TodoApi.Common.Contracts.Authentication;

namespace TodoApp.TodoApi.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<(Guid UserId, RefreshRequest Request), RefreshCommand>()
			.Map(dest => dest.UserId, src => src.UserId)
			.Map(dest => dest.RefreshToken, src => src.Request.RefreshToken);
	}
}