using FluentAssertions;
using PMS.Application.ProjectTasks.Commands.CreateProjectTask;
using Xunit;

namespace PMS.Application.Tests.ProjectTasks.Commands;

public class CreateProjectTaskCommandValidatorTests
{
    private readonly CreateProjectTaskCommandValidator _validator;

    public CreateProjectTaskCommandValidatorTests()
    {
        _validator = new CreateProjectTaskCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_ProjectId_Is_Empty()
    {
        // Arrange
        var command = new CreateProjectTaskCommand
        {
            ProjectId = Guid.Empty,
            Title = "Test Task",
            Description = "Test Description",
            TaskKey = "TST",
            ReporterId = Guid.NewGuid(),
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "ProjectId");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        // Arrange
        var command = new CreateProjectTaskCommand
        {
            ProjectId = Guid.NewGuid(),
            Title = "",
            Description = "Test Description",
            TaskKey = "TST",
            ReporterId = Guid.NewGuid(),
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Title");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateProjectTaskCommand
        {
            ProjectId = Guid.NewGuid(),
            Title = new string('a', 201),
            Description = "Test Description",
            TaskKey = "TST",
            ReporterId = Guid.NewGuid(),
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "Title");
    }

    [Fact]
    public void Should_Have_Error_When_TaskKey_Is_Not_Uppercase()
    {
        // Arrange
        var command = new CreateProjectTaskCommand
        {
            ProjectId = Guid.NewGuid(),
            Title = "Test Task",
            Description = "Test Description",
            TaskKey = "tst",
            ReporterId = Guid.NewGuid(),
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == "TaskKey");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateProjectTaskCommand
        {
            ProjectId = Guid.NewGuid(),
            Title = "Test Task",
            Description = "Test Description",
            TaskKey = "TST",
            ReporterId = Guid.NewGuid(),
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
