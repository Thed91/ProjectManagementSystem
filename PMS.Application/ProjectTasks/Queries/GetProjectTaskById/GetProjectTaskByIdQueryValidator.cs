using FluentValidation;

namespace PMS.Application.ProjectTasks.Queries.GetProjectTaskById;

public class GetProjectTaskByIdQueryValidator : AbstractValidator<GetProjectTaskByIdQuery>
{
    public GetProjectTaskByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
