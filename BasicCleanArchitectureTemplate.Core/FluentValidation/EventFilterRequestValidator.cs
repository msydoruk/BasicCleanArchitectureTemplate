using BasicCleanArchitectureTemplate.Core.Models;
using FluentValidation;

namespace BasicCleanArchitectureTemplate.Core.FluentValidation
{
    public class EventFilterRequestValidator : AbstractValidator<EventFilterRequest>
    {
        public EventFilterRequestValidator()
        {
            RuleFor(eventFilter => eventFilter.StartDate).NotNull()
                .WithMessage("Event filter Start Date cannot be empty.");

            RuleFor(eventFilter => eventFilter.EndDate).NotNull()
                .WithMessage("Event filter End Date cannot be empty.");
        }
    }
}
