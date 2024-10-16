using FluentValidation;
using BasicCleanArchitectureTemplate.Core.Interfaces;

namespace BasicCleanArchitectureTemplate.Core.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IValidatorServiceFactory _validatorServiceFactory;

        public ValidationService(IValidatorServiceFactory validatorServiceFactory)
        {
            _validatorServiceFactory = validatorServiceFactory;
        }

        public async Task ValidateModel<TModel>(TModel model)
        {
            var validator = _validatorServiceFactory.GetValidator<TModel>();
            if (validator == null)
            {
                throw new InvalidOperationException($"No validator found for type {typeof(TModel).Name}");
            }

            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
