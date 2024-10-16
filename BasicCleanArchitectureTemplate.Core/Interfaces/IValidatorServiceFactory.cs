using FluentValidation;

namespace BasicCleanArchitectureTemplate.Core.Interfaces
{
    public interface IValidatorServiceFactory
    {
        IValidator<TModel> GetValidator<TModel>();
    }
}
