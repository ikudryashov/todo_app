using Moq;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Users.Commands.Update;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UnitTests.Users;

public class UpdateUserCommandHandlerTests
{
	private readonly Mock<IUserRepository> _mockUserRepository;
	private readonly UpdateUserCommandHandler _handler;


	public UpdateUserCommandHandlerTests()
	{
		_mockUserRepository = new Mock<IUserRepository>();
		_handler = new UpdateUserCommandHandler(_mockUserRepository.Object);
	}

	[Fact]
	public async Task HandleUpdateUserCommand_WhenUserExists_ShouldUpdateUser()
	{
		//Arrange
		var id = Guid.NewGuid();
		var command = new UpdateUserCommand(
			id,
			id,
			"New first name",
			"New last name",
			"New email"
			);
		var user = new User
		{
			Id = id,
			FirstName = "First name",
			LastName = "Last name",
			Email = "Email"
		};

		_mockUserRepository.Setup(
			x => x.GetUserById(command.Id)).ReturnsAsync(user);
		_mockUserRepository.Setup(
			x => x.UpdateUser(It.IsAny<User>())).Returns(Task.CompletedTask);
		
		//Act
		await _handler.Handle(command, CancellationToken.None);
		
		//Assert
		_mockUserRepository.Verify(x => x.GetUserById(command.Id), Times.Once);
		_mockUserRepository.Verify(x => x.UpdateUser(
			It.Is<User>(u => u.Id == command.Id 
												&& u.FirstName == command.FirstName 
			                                    && u.LastName == command.LastName 
												&& u.Email == command.Email)), Times.Once);
	}

	[Fact]
	public async Task HandleUpdateUserCommand_WhenUserDoesNotExist_ShouldThrowNotFoundException()
	{
		//Arrange
		var id = Guid.NewGuid();
		var command = new UpdateUserCommand(
			id,
			id,
			"New first name",
			"New last name",
			"New email"
			);

		_mockUserRepository.Setup(x => x.GetUserById(command.Id)).ReturnsAsync((User)null);
		
		//Act
		var ex = await Assert.ThrowsAsync<ApiException>(
			() => _handler.Handle(command, CancellationToken.None));
		
		//Assert
		Assert.Equal("Not found", ex.Title);
		Assert.Equal("User does not exist.", ex.Detail);
	}

	[Fact]
	public async Task HandleUpdateUserCommand_WhenIdDoesNotMatch_ShouldThrowUnauthorizedException()
	{
		//Arrange
		var userId = Guid.NewGuid();
		var requestId = Guid.NewGuid();
		var command = new UpdateUserCommand(
			userId, requestId, "First Name",  "Last Name", "Email");

		var user = new User { Id = userId };
		
		_mockUserRepository.Setup(x => x.GetUserById(command.Id)).ReturnsAsync(user);
		
		//Act
		var ex = await Assert.ThrowsAsync<ApiException>(
			() => _handler.Handle(command, CancellationToken.None));
		
		//Assert
		Assert.Equal("Unauthorized", ex.Title);
		Assert.Equal("You do not have access to this resource.", ex.Detail);
		_mockUserRepository.Verify(x => x.GetUserById(command.Id), Times.Never);
		_mockUserRepository.Verify(x => x.UpdateUser(It.IsAny<User>()), Times.Never);
	}
}