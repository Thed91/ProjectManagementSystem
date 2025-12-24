using FluentValidation;

namespace PMS.Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator() {
            RuleFor(x => x.Name)
                   .NotEmpty().WithMessage("Name is required")
                   .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required")
                    .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");
            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}
