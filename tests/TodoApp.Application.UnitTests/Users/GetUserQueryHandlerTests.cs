using Moq;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Common.Interfaces.Persistence;
using TodoApp.Application.Users.Queries;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.UnitTests.Users;

public class GetUserQueryHandlerTests
{
	private readonly Mock<IUserRepository> _mockUserRepository;
	private readonly GetUserQueryHandler _handler;

	public GetUserQueryHandlerTests()
	{
		_mockUserRepository = new Mock<IUserRepository>();
		_handler = new GetUserQueryHandler(_mockUserRepository.Object);
	}

	[Fact]
	public async Task HandleGetUserQuery_WhenUserExists_ShouldReturnUser()
	{
		//Arrange
		var id = Guid.NewGuid();
		var query = new GetUserQuery(id, id);
		
		var user = new User { Id = id };
		
		_mockUserRepository.Setup(x => x.GetUserById(query.Id)).ReturnsAsync(user);
		
		//Act
		var result = await _handler.Handle(query, CancellationToken.None);
		
		//Assert
		_mockUserRepository.Verify(x => x.GetUserById(query.Id), Times.Once);
		Assert.NotNull(result);
		Assert.Equal(query.Id, result.Id);
	}

	[Fact]
	public async Task HandleGetUserQuery_WhenUserDoesNotExist_ShouldThrowNotFoundException()
	{
		//Arrange
		var id = Guid.NewGuid();
		var query = new GetUserQuery(id, id);

		_mockUserRepository.Setup(x => x.GetUserById(query.Id)).ReturnsAsync((User)null);
		
		//Act
		var ex = await Assert.ThrowsAsync<ApiException>(
			 () =>  _handler.Handle(query, CancellationToken.None));
		
		//Assert
		Assert.Equal("Not found", ex.Title);
		Assert.Equal("User does not exist", ex.Detail);
	}
	
	[Fact]
	public async Task HandleGetUserQuery_WhenIdDoesNotMatch_ShouldThrowUnauthorizedException()
	{
		//Arrange
		var userId = Guid.NewGuid();
		var requestId = Guid.NewGuid();
		var query = new GetUserQuery(userId, requestId);

		var user = new User { Id = userId };
		
		_mockUserRepository.Setup(x => x.GetUserById(query.Id)).ReturnsAsync(user);
		
		//Act
		var ex = await Assert.ThrowsAsync<ApiException>(
			() =>  _handler.Handle(query, CancellationToken.None));
		
		//Assert
		Assert.Equal("Unauthorized", ex.Title);
		Assert.Equal("You do not have access to this resource.", ex.Detail);
		_mockUserRepository.Verify(x => x.GetUserById(query.Id), Times.Never);
	}
}