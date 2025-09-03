using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using SubscriptionService.Application.UseCases.Commands.GetOrCreateUserCommand;
using SubscriptionService.Domain.Abstractions;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Domain.Entities.UserAggregate;

namespace UnitTests.Application.UseCases.GetOrCreateUser;

[TestSubject(typeof(GetOrCreateUserHandler))]
public class GetOrCreateUserHandlerShould
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetOrCreateUserHandler _handler;
    
    public GetOrCreateUserHandlerShould()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new GetOrCreateUserHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnExistingUser_WhenUserWithTelegramIdAlreadyExists()
    {
        //Arrange
        var existingUser = User.Create(TelegramId.Create(123), "228", UserStatus.Active);
        var command = new GetOrCreateUserCommand(existingUser.TelegramId.Value);
        
        _userRepositoryMock
            .Setup(r => r.GetByTelegramIdAsync(command.TelegramId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be(existingUser.Id);
        
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_CreateAndReturnNewUser_WhenUserWithTelegramIdDoesNotExist()
    {
        //Arrange
        var command = new GetOrCreateUserCommand(228);
        _userRepositoryMock
            .Setup(r => r.GetByTelegramIdAsync(command.TelegramId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().NotBeEmpty();
        result.Value.TelegramId.Should().Be(command.TelegramId);

        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}