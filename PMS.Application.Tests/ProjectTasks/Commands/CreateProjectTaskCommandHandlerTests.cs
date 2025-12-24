using AutoMapper;
using FluentAssertions;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Mappings;
using PMS.Application.ProjectTasks.Commands.CreateProjectTask;
using PMS.Application.Tests.Common;
using Xunit;

namespace PMS.Application.Tests.ProjectTasks.Commands;

public class CreateProjectTaskCommandHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly CreateProjectTaskCommandHandler _handler;

    public CreateProjectTaskCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();

        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
        _handler = new CreateProjectTaskCommandHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Create_ProjectTask_Successfully()
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
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ProjectId.Should().Be(command.ProjectId);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.TaskKey.Should().Be(command.TaskKey);
        result.ReporterId.Should().Be(command.ReporterId);
        result.CreatedBy.Should().Be(command.CreatedBy);
        result.Status.Should().Be("ToDo");
        result.Priority.Should().Be("Medium");
        result.Type.Should().Be("Task");
    }

    [Fact]
    public async Task Handle_Should_Set_Default_Values()
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
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Status.Should().Be("ToDo");
        result.Priority.Should().Be("Medium");
        result.Type.Should().Be("Task");
        result.AssigneeId.Should().BeNull();
        result.EstimatedHours.Should().BeNull();
        result.ActualHours.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Generate_New_Id()
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
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().NotBeEmpty();
    }

    public void Dispose()
    {
        TestDbContextFactory.Destroy(_context);
    }
}
