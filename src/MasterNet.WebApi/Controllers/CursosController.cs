using MasterNet.Application.Core;
using MasterNet.Application.Cursos.CursoCreate;
using MasterNet.Application.Cursos.GetCursos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static MasterNet.Application.Cursos.CursoCreate.CursoCreateCommand;
using static MasterNet.Application.Cursos.CursoReporteExcel.CursoReporteExcelQuery;
using static MasterNet.Application.Cursos.GetCurso.GetCursoQuery;
using static MasterNet.Application.Cursos.GetCursos.GetCursosQuery;

namespace MasterNet.WebApi.Controllers;


[ApiController]
[Route("api/cursos")]
public class CursosController : ControllerBase
{
    private readonly ISender _sender;

    public CursosController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Result<Guid>>> CursoCreate(
        [FromForm] CursoCreateRequest request,
        CancellationToken cancellationToken
    )
    {

        var command = new CursoCreateCommandRequest(request);
        return await _sender.Send(command, cancellationToken);


    }

    [HttpGet]
    public async Task<IActionResult> PaginationCursos(
        [FromQuery] GetCursosRequest request,
        CancellationToken cancellationToken
    ){
        var query = new GetCursosQueryRequest{
            CursosRequest = request
        };

        var resultados = await _sender.Send(query,cancellationToken);

        return resultados.IsSuccess ? Ok(resultados.Value) : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> CursoGet(Guid id,CancellationToken cancellationToken)
    {
        var query = new GetCursoQueryRequest{
            Id = id
        };

        var result = await _sender.Send(query,cancellationToken);

        return result.IsSuccess ? Ok(result.Value):BadRequest();
    }

    [HttpGet("reporte")]
    public async Task<IActionResult> ReporteCSV(CancellationToken cancellationToken)
    {
        var query = new CursoReporteExcelQueryRequest();
        var resultado = await _sender.Send(query, cancellationToken);

        byte[] excelBytes = resultado.ToArray();

        return File(excelBytes, "text/csv", "cursos.csv");

    }
}