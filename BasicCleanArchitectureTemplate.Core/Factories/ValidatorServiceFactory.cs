using BasicCleanArchitectureTemplate.Core.Interfaces;
using FluentValidation;

namespace BasicCleanArchitectureTemplate.Core.Factories
{
    public class ValidatorServiceFactory : IValidatorServiceFactory
    {
        private readonly IEnumerable<IValidator> _validators;

        public ValidatorServiceFactory(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public IValidator<TModel> GetValidator<TModel>()
        {
            return _validators.OfType<IValidator<TModel>>().FirstOrDefault();
        }
    }
}
