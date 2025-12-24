using AutoMapper;
using FluentAssertions;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Mappings;
using PMS.Application.Projects.Queries.GetProjectById;
using PMS.Application.Tests.Common;
using PMS.Domain.Entities;
using Xunit;

namespace PMS.Application.Tests.Projects.Queries;

public class GetProjectByIdQueryHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetProjectByIdQueryHandler _handler;

    public GetProjectByIdQueryHandlerTests()
    {
        _context = TestDbContextFactory.Create();

        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
        _handler = new GetProjectByIdQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Project_When_Found()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var project = Project.Create("Test Project", "Test Description", "TEST", createdBy);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync(CancellationToken.None);

        var query = new GetProjectByIdQuery { Id = project.Id };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(project.Id);
        result.Name.Should().Be("Test Project");
        result.Description.Should().Be("Test Description");
        result.Key.Should().Be("TEST");
        result.Status.Should().Be("Planning");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Project_Not_Found()
    {
        // Arrange
        var query = new GetProjectByIdQuery { Id = Guid.NewGuid() };

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var project = Project.Create("Test Project", "Test Description", "TEST", createdBy);
        _context.Projects.Add(project);
        await _context.SaveChangesAsync(CancellationToken.None);

        var query = new GetProjectByIdQuery { Id = project.Id };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(project.Id);
        result.Name.Should().Be(project.Name);
        result.Description.Should().Be(project.Description);
        result.Key.Should().Be(project.Key);
        result.Status.Should().Be(project.Status.ToString());
        result.CreatedBy.Should().Be(project.CreatedBy);
        result.CreatedAt.Should().Be(project.CreatedAt);
    }

    public void Dispose()
    {
        TestDbContextFactory.Destroy(_context);
    }
}
