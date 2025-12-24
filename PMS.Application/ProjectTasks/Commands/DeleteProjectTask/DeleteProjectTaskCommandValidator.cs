using FluentValidation;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.DeleteProjectTask;

public class DeleteProjectTaskCommandValidator : AbstractValidator<DeleteProjectTaskCommand>
{
    public DeleteProjectTaskCommandValidator()
    {
        RuleFor(x => x.ModifiedBy).NotEmpty().WithMessage("Id is required");
    }
}