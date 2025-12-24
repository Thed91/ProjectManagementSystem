using FluentAssertions;
using PMS.Application.Projects.Commands.CreateProject;
using Xunit;

namespace PMS.Application.Tests.Projects.Commands;

public class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator;

    public CreateProjectCommandValidatorTests()
    {
        _validator = new CreateProjectCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "",
            Description = "Test description",
            Key = "TEST",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = new string('a', 201),
            Description = "Test description",
            Key = "TEST",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "",
            Key = "TEST",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Description");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Is_Not_Uppercase()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test description",
            Key = "test",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Key");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Is_Too_Short()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test description",
            Key = "T",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Key");
    }

    [Fact]
    public void Should_Have_Error_When_CreatedBy_Is_Empty()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test description",
            Key = "TEST",
            CreatedBy = Guid.Empty
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "CreatedBy");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test description",
            Key = "TEST",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
