using Moq;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Users.Commands.Delete;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UnitTests.Users;

public class DeleteUserCommandHandlerTests
{
	private readonly Mock<IUserRepository> _mockUserRepository;
	private readonly DeleteUserCommandHandler _handler;


	public DeleteUserCommandHandlerTests()
	{
		_mockUserRepository = new Mock<IUserRepository>();
		_handler = new DeleteUserCommandHandler(_mockUserRepository.Object);
	}

	[Fact]
	public async Task HandleDeleteUserCommand_WhenUserExists_ShouldDeleteUser()
	{
		//Arrange
		var id = Guid.NewGuid();
		var command = new DeleteUserCommand(id, id);
		var user = new User { Id = id };
		
		_mockUserRepository.Setup(
			x => x.GetUserById(command.Id)).ReturnsAsync(user);
		_mockUserRepository.Setup(
			x => x.DeleteUser(id)).Returns(Task.CompletedTask);
		
		//Act
		await _handler.Handle(command, CancellationToken.None);
		
		//Assert
		_mockUserRepository.Verify(x => x.GetUserById(command.Id), Times.Once);
		_mockUserRepository.Verify(x => x.DeleteUser(command.Id), Times.Once);
	}

	[Fact]
	public async Task HandleDeleteUserCommand_WhenUserDoesNotExist_ShouldThrowNotFoundException()
	{
		//Arrange
		var id = Guid.NewGuid();
		var command = new DeleteUserCommand(id, id);
		_mockUserRepository.Setup(
			x => x.GetUserById(command.Id)).ReturnsAsync((User)null);
		
		//Act
		var ex = await Assert.ThrowsAsync<ApiException>(
			() => _handler.Handle(command, CancellationToken.None));
		
		//Assert
		Assert.Equal("Not found", ex.Title);
		Assert.Equal("User does not exist", ex.Detail);
	}

	[Fact]
	public async Task HandleDeleteUserCommand_WhenIdDoesNotMatch_ShouldThrowUnauthorizedError()
	{
		//Arrange
		var userId = Guid.NewGuid();
		var requestId = Guid.NewGuid();
		var command = new DeleteUserCommand(userId, requestId);
		
		//Act
		var ex = await Assert.ThrowsAsync<ApiException>(
			() => _handler.Handle(command, CancellationToken.None));
		
		//Assert
		Assert.Equal("Unauthorized", ex.Title);
		Assert.Equal("You do not have access to this resource.", ex.Detail);
		_mockUserRepository.Verify(x => x.GetUserById(command.Id), Times.Never);
		_mockUserRepository.Verify(x => x.DeleteUser(command.Id), Times.Never);
		
	}
}