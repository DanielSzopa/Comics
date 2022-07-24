using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Comics.ApplicationCore.Features.Registration;

public static class RegistrationDependencyInjectionExtension
{
    public static IServiceCollection AddRegistrationServices(this IServiceCollection service)
    {
        service.AddScoped<IValidator<RegisterAccountRequest>, RegisterAccountRequestValidator>();
        return service;
    }
}
