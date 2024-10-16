using BasicCleanArchitectureTemplate.Core.Models;
using FluentValidation;

namespace BasicCleanArchitectureTemplate.Core.FluentValidation
{
    public class EventModelValidator : AbstractValidator<EventModel>
    {
        public EventModelValidator()
        {
            RuleFor(@event => @event.Id).Must(id => id != Guid.Empty).WithMessage("Event Id cannot be empty.");
        }
    }
}
