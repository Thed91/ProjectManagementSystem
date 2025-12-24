using FluentValidation;

namespace PMS.Application.ProjectTasks.Queries.GetProjectTasks;

public class GetProjectTasksQueryValidator : AbstractValidator<GetProjectTasksQuery>
{
    public GetProjectTasksQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page Number must be >= 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page Size must be >= 1")
            .LessThanOrEqualTo(100)
            .WithMessage("Page Size should not exceed 100");
    }
}
