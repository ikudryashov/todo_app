using MediatR;
using TodoApp.Application.Users.Common;

namespace TodoApp.Application.Users.Queries;

public record GetUserQuery(Guid Id) : IRequest<UserResult>;