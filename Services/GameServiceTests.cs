using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Playmor_Asp.Application.DTOs;
using Playmor_Asp.Application.Interfaces;
using Playmor_Asp.Application.Services;
using Playmor_Asp.Domain.Models;

namespace Playmor_Asp.Tests.Services;
public class GameServiceTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<Game>> _gameValidatorMock;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _mapperMock = new Mock<IMapper>();
        _gameValidatorMock = new Mock<IValidator<Game>>();
        _gameService = new GameService(_gameRepositoryMock.Object, _mapperMock.Object, _gameValidatorMock.Object);
    }

    [Fact]
    public async Task CreateGameAsync_ShouldReturnTrue_WhenGameDTOIsValid()
    {
        // Arrange
        var gameDto = A.Fake<GameDTO>();
        var game = A.Fake<Game>();

        _mapperMock.Setup(m => m.Map<Game>(gameDto)).Returns(game);
        _gameValidatorMock.Setup(m => m.Validate(game)).Returns(new ValidationResult());
        _gameRepositoryMock.Setup(repo => repo.CreateAsync(game)).ReturnsAsync(true);

        // Act
        var result = await _gameService.CreateGameAsync(gameDto);

        // Assert
        result.Data.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateGameAsync_ShouldReturnFalse_WhenGameDTOIsInvalid()
    {
        // Arrange
        var gameDto = A.Fake<GameDTO>();
        var game = A.Fake<Game>();

        _mapperMock.Setup(m => m.Map<Game>(gameDto)).Returns(game);
        _gameValidatorMock.Setup(m => m.Validate(game)).Returns(new ValidationResult());
        _gameRepositoryMock.Setup(repo => repo.CreateAsync(game)).ReturnsAsync(false);

        // Act
        var result = await _gameService.CreateGameAsync(gameDto);

        // Assert
        result.Data.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DeleteGameAsync_ShouldReturnTrue_WhenGameFound()
    {
        // Arrange
        var id = 1;
        var game = A.Fake<Game>();

        _gameRepositoryMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(game);
        _gameRepositoryMock.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _gameService.DeleteGameAsync(id);

        // Assert
        result.Data.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteGameAsync_ShouldReturnFalse_WhenGameNotFound()
    {
        // Arrange
        var id = 1;
        _gameRepositoryMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(null as Game);

        // Act
        var result = await _gameService.DeleteGameAsync(id);

        // Assert
        result.Data.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetGameAsync_ShouldReturnGame_WhenGameFound()
    {
        // Arrange
        var id = 1;
        var game = A.Fake<Game>();
        _gameRepositoryMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(game);

        // Act
        var result = await _gameService.GetGameAsync(id);

        // Assert
        result.Data.Should().Be(game);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGameAsync_ShouldNotReturnGame_WhenGameNotFound()
    {
        // Arrange
        var id = 1;
        _gameRepositoryMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(null as Game);

        // Act
        var result = await _gameService.GetGameAsync(id);

        // Assert
        result.Data.Should().BeNull();
        result.Errors.Should().NotBeEmpty();
    }


}
