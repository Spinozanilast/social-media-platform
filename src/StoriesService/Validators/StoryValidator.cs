using FluentValidation;
using StoriesService.Entities;

namespace StoriesService.Validators;

public class StoryValidator : AbstractValidator<Story>
{
    public StoryValidator()
    {
        RuleFor(story => story.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(story => story.Content).NotEmpty().WithMessage("Content is required");
    }
}