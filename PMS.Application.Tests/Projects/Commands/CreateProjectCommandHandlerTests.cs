using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Mappings;
using PMS.Application.Projects.Commands.CreateProject;
using PMS.Application.Tests.Common;
using Xunit;

namespace PMS.Application.Tests.Projects.Commands;

public class CreateProjectCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();

        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
        _handler = new CreateProjectCommandHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Create_Project_Successfully()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test Description",
            Key = "TEST",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.Key.Should().Be(command.Key.ToUpper());
        result.CreatedBy.Should().Be(command.CreatedBy);
        result.Status.Should().Be("Planning");

        var projectInDb = await _context.Projects.FirstOrDefaultAsync(p => p.Id == result.Id);
        projectInDb.Should().NotBeNull();
        projectInDb!.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_Should_Set_Default_Status_To_Planning()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test Description",
            Key = "TEST",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Status.Should().Be("Planning");
    }

    [Fact]
    public async Task Handle_Should_Convert_Key_To_Uppercase()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test Description",
            Key = "test",
            CreatedBy = Guid.NewGuid()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Key.Should().Be("TEST");
    }

    [Fact]
    public async Task Handle_Should_Set_CreatedAt_Timestamp()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test Description",
            Key = "TEST",
            CreatedBy = Guid.NewGuid()
        };

        var beforeCreate = DateTime.UtcNow;

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        var afterCreate = DateTime.UtcNow;

        // Assert
        result.CreatedAt.Should().BeAfter(beforeCreate.AddSeconds(-1));
        result.CreatedAt.Should().BeBefore(afterCreate.AddSeconds(1));
    }

    public void Dispose()
    {
        TestDbContextFactory.Destroy(_context);
    }
}
