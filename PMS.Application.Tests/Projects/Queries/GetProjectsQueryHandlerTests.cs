using AutoMapper;
using FluentAssertions;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Mappings;
using PMS.Application.Projects.Queries.GetProjects;
using PMS.Application.Tests.Common;
using PMS.Domain.Entities;
using Xunit;

namespace PMS.Application.Tests.Projects.Queries;

public class GetProjectsQueryHandlerTests : IDisposable
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetProjectsQueryHandler _handler;

    public GetProjectsQueryHandlerTests()
    {
        _context = TestDbContextFactory.Create();

        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configurationProvider.CreateMapper();
        _handler = new GetProjectsQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Paginated_Projects()
    {
        // Arrange
        var createdBy = Guid.NewGuid();

        for (int i = 1; i <= 15; i++)
        {
            var project = Project.Create($"Project {i}", $"Description {i}", $"PRJ{i}", createdBy);
            _context.Projects.Add(project);
        }

        await _context.SaveChangesAsync(CancellationToken.None);

        var query = new GetProjectsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(10);
        result.TotalCount.Should().Be(15);
        result.PageNumber.Should().Be(1);
        result.TotalPages.Should().Be(2);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Second_Page_Correctly()
    {
        // Arrange
        var createdBy = Guid.NewGuid();

        for (int i = 1; i <= 15; i++)
        {
            var project = Project.Create($"Project {i}", $"Description {i}", $"PRJ{i}", createdBy);
            _context.Projects.Add(project);
        }

        await _context.SaveChangesAsync(CancellationToken.None);

        var query = new GetProjectsQuery
        {
            PageNumber = 2,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(5);
        result.PageNumber.Should().Be(2);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Projects()
    {
        // Arrange
        var query = new GetProjectsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_Use_Default_PageSize()
    {
        // Arrange
        var createdBy = Guid.NewGuid();

        for (int i = 1; i <= 5; i++)
        {
            var project = Project.Create($"Project {i}", $"Description {i}", $"PRJ{i}", createdBy);
            _context.Projects.Add(project);
        }

        await _context.SaveChangesAsync(CancellationToken.None);

        var query = new GetProjectsQuery(); // Default PageNumber = 1, PageSize = 10

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(5);
        result.PageNumber.Should().Be(1);
    }

    public void Dispose()
    {
        TestDbContextFactory.Destroy(_context);
    }
}
