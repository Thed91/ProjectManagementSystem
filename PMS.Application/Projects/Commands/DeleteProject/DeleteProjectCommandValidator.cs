using FluentValidation;

namespace PMS.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator() {
            RuleFor(x => x.ModifiedBy).NotEmpty().WithMessage("Id is required");
        }
    }
}
