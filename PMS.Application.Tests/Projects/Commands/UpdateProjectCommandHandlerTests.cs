using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Mappings;
using PMS.Application.Projects.Commands.UpdateProject;
using PMS.Application.Tests.Common;
using PMS.Domain.Entities;
using Xunit;

namespace PMS.Application.Tests.Projects.Commands;

public class UpdateProjectCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UpdateProjectCommandHandler _handler;

    public UpdateProjectCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();

        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
        _handler = new UpdateProjectCommandHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Update_Project_Successfully()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var modifiedBy = Guid.NewGuid();

        var project = Project.Create("Original Name", "Original Description", "ORIG", createdBy);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new UpdateProjectCommand
        {
            Id = project.Id,
            Name = "Updated Name",
            Description = "Updated Description",
            ModifiedBy = modifiedBy
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(project.Id);
        result.Name.Should().Be("Updated Name");
        result.Description.Should().Be("Updated Description");
        result.LastModifiedBy.Should().Be(modifiedBy);
        result.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Project_Not_Found()
    {
        // Arrange
        var command = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Name",
            Description = "Updated Description",
            ModifiedBy = Guid.NewGuid()
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Not_Update_Key()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var project = Project.Create("Original Name", "Original Description", "ORIG", createdBy);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync(CancellationToken.None);

        var originalKey = project.Key;

        var command = new UpdateProjectCommand
        {
            Id = project.Id,
            Name = "Updated Name",
            Description = "Updated Description",
            ModifiedBy = Guid.NewGuid()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Key.Should().Be(originalKey);
    }

    [Fact]
    public async Task Handle_Should_Preserve_CreatedBy_And_CreatedAt()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var project = Project.Create("Original Name", "Original Description", "ORIG", createdBy);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync(CancellationToken.None);

        var originalCreatedAt = project.CreatedAt;
        var originalCreatedBy = project.CreatedBy;

        var command = new UpdateProjectCommand
        {
            Id = project.Id,
            Name = "Updated Name",
            Description = "Updated Description",
            ModifiedBy = Guid.NewGuid()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.CreatedAt.Should().Be(originalCreatedAt);
        result.CreatedBy.Should().Be(originalCreatedBy);
    }

    public void Dispose()
    {
        TestDbContextFactory.Destroy(_context);
    }
}
