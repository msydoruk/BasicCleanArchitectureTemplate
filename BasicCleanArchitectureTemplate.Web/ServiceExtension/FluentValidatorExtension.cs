using BasicCleanArchitectureTemplate.Core.Factories;
using BasicCleanArchitectureTemplate.Core.FluentValidation;
using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;
using FluentValidation;

namespace BasicCleanArchitectureTemplate.Web.ServiceExtension
{
    public static class FluentValidatorExtension
    {
        public static IServiceCollection AddCustomValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<EventModel>, EventModelValidator>();
            services.AddTransient<IValidator<EventFilterRequest>, EventFilterRequestValidator>();
            services.AddTransient<IValidator<EventIdRequest>, EventIdRequestValidator>();

            return services;
        }

        public static IServiceCollection RegisterCustomValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidatorServiceFactory>(provider =>
            {
                var validators = provider.GetServices<IValidator<EventModel>>()
                    .Cast<IValidator>()
                    .Concat(provider.GetServices<IValidator<EventFilterRequest>>())
                    .Concat(provider.GetServices<IValidator<EventIdRequest>>())
                    .ToList();
                return new ValidatorServiceFactory(validators);
            });

            return services;
        }
    }
}
