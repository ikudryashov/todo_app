using MediatR;

namespace TodoApp.Application.Authentication.Commands.LogOut;

public record LogOutCommand
(Guid UserId) : IRequest;