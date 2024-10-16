using BasicCleanArchitectureTemplate.Core.Models;
using FluentValidation;

namespace BasicCleanArchitectureTemplate.Core.FluentValidation
{
    public class EventIdRequestValidator : AbstractValidator<EventIdRequest>
    {
        public EventIdRequestValidator()
        {
            RuleFor(@event => @event.Id)
                .NotEmpty().WithMessage("Event Id cannot be empty.")
                .NotEqual(Guid.Empty).WithMessage("Event id cannot be the default GUID.");
        }
    }
}
