using FluentValidation;

namespace PMS.Application.Projects.Queries.GetProjects
{
    public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
    {
        public GetProjectsQueryValidator()
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
}
