namespace BasicCleanArchitectureTemplate.Core.Interfaces
{
    public interface IValidationService
    {
        Task ValidateModel<TModel>(TModel model);
    }
}
