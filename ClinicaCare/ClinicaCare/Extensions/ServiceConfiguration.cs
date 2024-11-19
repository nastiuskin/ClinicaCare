using Application.Auth;
using Application.MedicalProcedureManagement.Commands.Create;
using AutoMapper;
using Domain.Validation;
using FluentValidation;

namespace API.Extensions
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateMedicalProcedureCommandHandler).Assembly));

            services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

            services.AddValidatorsFromAssembly(typeof(MedicalProcedureCreateDtoValidator).Assembly);

            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IMapper, Mapper>();

            return services;
        }
    }
}

