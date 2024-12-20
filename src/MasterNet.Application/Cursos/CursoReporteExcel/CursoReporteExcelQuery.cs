using MasterNet.Application.Interfaces;
using MasterNet.Domain;
using MasterNet.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MasterNet.Application.Cursos.CursoReporteExcel;


public class CursoReporteExcelQuery
{

    public record CursoReporteExcelQueryRequest 
        : IRequest<MemoryStream>;

    internal class CursoReporteExcelQueryHandler
    : IRequestHandler<CursoReporteExcelQueryRequest, MemoryStream>
    {
        private readonly MasterNetDbContext _context;
        private readonly IReportService<Curso> _reporteService;

        public CursoReporteExcelQueryHandler(
            MasterNetDbContext context, 
            IReportService<Curso> reporteService
        )
        {
            _context = context;
            _reporteService = reporteService;
        }

        public async Task<MemoryStream> Handle(
            CursoReporteExcelQueryRequest request, 
            CancellationToken cancellationToken
        )
        {
            var cursos = await _context.Cursos!.Take(10).Skip(0).ToListAsync();

            return await _reporteService.GetCsvReport(cursos);
        }
    }




}