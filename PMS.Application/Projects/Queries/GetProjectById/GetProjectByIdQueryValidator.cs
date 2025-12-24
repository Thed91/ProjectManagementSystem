using FluentValidation;

namespace PMS.Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
    {
        public GetProjectByIdQueryValidator() {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }
}
