using FluentValidation;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.CreateProjectTask;

public class CreateProjectTaskCommandValidator : AbstractValidator<ProjectTaskDto>
{
    public CreateProjectTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");
        
        RuleFor(x => x.TaskKey)
            .NotEmpty().WithMessage("Key is required")
            .Length(2, 10).WithMessage("Key must be between 2 and 10 characters")
            .Matches("^[A-Z]+$").WithMessage("Key must contain only uppercase letters");
       
        RuleFor(x => x.ReporterId)
            .NotEmpty().WithMessage("ReporterId is required");
       
        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CreatedBy is required");
    }
}