// Command
// CommandHandler

using FluentValidation;
using MasterNet.Application.Core;
using MasterNet.Domain;
using MasterNet.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace MasterNet.Application.Cursos.CursoCreate;

public class CursoCreateCommand
{
    public record CursoCreateCommandRequest(CursoCreateRequest cursoCreateRequest) 
    : IRequest<Result<Guid>>;


    internal class CursoCreateCommandHandler
    : IRequestHandler<CursoCreateCommandRequest, Result<Guid>>
    {
        private readonly MasterNetDbContext _context;

        public CursoCreateCommandHandler(MasterNetDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<Guid>> Handle(
            CursoCreateCommandRequest request, 
            CancellationToken cancellationToken
        )
        {
            
            var curso = new Curso {
                Id = Guid.NewGuid(),
                Titulo = request.cursoCreateRequest.Titulo,
                Descripcion = request.cursoCreateRequest.Descripcion,
                FechaPublicacion = request.cursoCreateRequest.FechaPublicacion
            };

            _context.Add(curso);

            var resultado = await _context.SaveChangesAsync(cancellationToken) > 0;
         

            return resultado 
                        ? Result<Guid>.Success(curso.Id)
                        : Result<Guid>.Failure("No se pudo insertar el curso");
            
        }
    }


    public class CursoCreateCommandRequestValidator
    : AbstractValidator<CursoCreateCommandRequest>
    {
        public CursoCreateCommandRequestValidator()
        {
            RuleFor(x => x.cursoCreateRequest).SetValidator(new CursoCreateValidator());
        }

    }

}
