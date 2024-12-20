using FluentValidation;
using FluentValidation.AspNetCore;
using MasterNet.Application.Core;
using MasterNet.Application.Cursos.CursoCreate;
using Microsoft.Extensions.DependencyInjection;

namespace MasterNet.Application;


public static class DependencyInjection{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    ){
        
        services.AddMediatR(config=>{
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CursoCreateCommand>();

        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        return services;
    }
}