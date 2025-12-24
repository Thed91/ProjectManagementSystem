using FluentValidation;

namespace PMS.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator() {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }
}
