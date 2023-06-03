using Mapster;
using TodoApp.Application.Todos.Commands.Create;
using TodoApp.Application.Todos.Commands.Delete;
using TodoApp.Application.Todos.Commands.Update;
using TodoApp.TodoApi.Common.Contracts.Todos;

namespace TodoApp.TodoApi.Common.Mapping;

public class TodosMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<(Guid UserId, CreateTodoRequest request), CreateTodoCommand>()
			.Map(dest => dest.UserId, src => src.UserId)
			.Map(dest => dest.Title, src => src.request.Title)
			.Map(dest => dest.Description, src => src.request.Description)
			.Map(dest => dest.DueDate, src => src.request.DueDate)
			.Map(dest => dest.IsComplete, src => src.request.IsComplete);

		config.NewConfig<(Guid TodoId, Guid UserId, UpdateTodoRequest request), UpdateTodoCommand>()
			.Map(dest => dest.Id, src => src.TodoId)
			.Map(dest => dest.UserId, src => src.UserId)
			.Map(dest => dest.Title, src => src.request.Title)
			.Map(dest => dest.Description, src => src.request.Description)
			.Map(dest => dest.DueDate, src => src.request.DueDate)
			.Map(dest => dest.IsComplete, src => src.request.IsComplete);

		config.NewConfig<(Guid TodoId, Guid UserId), DeleteTodoCommand>()
			.Map(dest => dest.Id, src => src.TodoId)
			.Map(dest => dest.UserId, src => src.UserId);
	}
}