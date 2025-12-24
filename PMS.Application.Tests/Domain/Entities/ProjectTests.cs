using FluentAssertions;
using PMS.Domain.Entities;
using PMS.Domain.Enums;
using PMS.Domain.Exceptions;
using Xunit;

namespace PMS.Application.Tests.Domain.Entities;

public class ProjectTests
{
    [Fact]
    public void Create_Should_Create_Project_With_Valid_Data()
    {
        // Arrange
        var name = "Test Project";
        var description = "Test Description";
        var key = "test";
        var createdBy = Guid.NewGuid();

        // Act
        var project = Project.Create(name, description, key, createdBy);

        // Assert
        project.Should().NotBeNull();
        project.Name.Should().Be(name);
        project.Description.Should().Be(description);
        project.Key.Should().Be("TEST"); // Should be uppercase
        project.Status.Should().Be(ProjectStatus.Planning);
        project.CreatedBy.Should().Be(createdBy);
        project.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Create_Should_Convert_Key_To_Uppercase()
    {
        // Arrange
        var key = "test";

        // Act
        var project = Project.Create("Name", "Description", key, Guid.NewGuid());

        // Assert
        project.Key.Should().Be("TEST");
    }

    [Fact]
    public void UpdateDetails_Should_Update_Name_And_Description()
    {
        // Arrange
        var project = Project.Create("Original", "Original Description", "TEST", Guid.NewGuid());
        var modifiedBy = Guid.NewGuid();

        // Act
        project.UpdateDetails("Updated", "Updated Description", modifiedBy);

        // Assert
        project.Name.Should().Be("Updated");
        project.Description.Should().Be("Updated Description");
        project.LastModifiedBy.Should().Be(modifiedBy);
        project.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Start_Should_Change_Status_To_Active_When_Planning()
    {
        // Arrange
        var project = Project.Create("Name", "Description", "TEST", Guid.NewGuid());
        var modifiedBy = Guid.NewGuid();

        // Act
        project.Start(modifiedBy);

        // Assert
        project.Status.Should().Be(ProjectStatus.Active);
        project.StartDate.Should().NotBeNull();
        project.LastModifiedBy.Should().Be(modifiedBy);
    }

    [Fact]
    public void Start_Should_Throw_When_Not_In_Planning_Status()
    {
        // Arrange
        var project = Project.Create("Name", "Description", "TEST", Guid.NewGuid());
        var modifiedBy = Guid.NewGuid();
        project.Start(modifiedBy); // Now Active

        // Act
        Action act = () => project.Start(modifiedBy);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("The project cannot be launched, Status Planning");
    }

    [Fact]
    public void Complete_Should_Change_Status_To_Completed()
    {
        // Arrange
        var project = Project.Create("Name", "Description", "TEST", Guid.NewGuid());
        var modifiedBy = Guid.NewGuid();

        // Act
        project.Complete(modifiedBy);

        // Assert
        project.Status.Should().Be(ProjectStatus.Completed);
        project.EndDate.Should().NotBeNull();
        project.LastModifiedBy.Should().Be(modifiedBy);
    }

    [Fact]
    public void Delete_Should_Mark_As_Deleted_And_Cancelled()
    {
        // Arrange
        var project = Project.Create("Name", "Description", "TEST", Guid.NewGuid());
        var modifiedBy = Guid.NewGuid();

        // Act
        project.Delete(modifiedBy);

        // Assert
        project.IsDeleted.Should().BeTrue();
        project.Status.Should().Be(ProjectStatus.Cancelled);
        project.LastModifiedBy.Should().Be(modifiedBy);
    }
}
