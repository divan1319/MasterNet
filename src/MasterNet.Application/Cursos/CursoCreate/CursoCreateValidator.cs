using FluentValidation;

namespace MasterNet.Application.Cursos.CursoCreate;

public class CursoCreateValidator : AbstractValidator<CursoCreateRequest>
{
    public CursoCreateValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty();
        RuleFor(x => x.Descripcion).NotEmpty();
    }
}