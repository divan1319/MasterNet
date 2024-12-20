using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MasterNet.Application.Core;
using MasterNet.Domain;
using MasterNet.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterNet.Application.Instructores.GetInstructores;

public class GetInstructoresQuery{

    public record GetInstructoresQueryRequest: IRequest<Result<PagedList<InstructorResponse>>>{
        public GetInstructoresRequest? InstructorRequest { get; set; }
    }

    internal class GetInstructoresQueryHandler :
     IRequestHandler<GetInstructoresQueryRequest, Result<PagedList<InstructorResponse>>>
    {
        private readonly MasterNetDbContext _context;
        private readonly IMapper _mapper;

        public GetInstructoresQueryHandler(MasterNetDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<PagedList<InstructorResponse>>> Handle(
            GetInstructoresQueryRequest request,
            CancellationToken cancellationToken
            )
        {
            IQueryable<Instructor> queryable = _context.Instructores!;

            var predicate = ExpressionBuilder.New<Instructor>();


            if(!string.IsNullOrEmpty(request.InstructorRequest!.Nombre)){
                predicate = predicate.And(y => y.Nombre!.Contains(request.InstructorRequest.Nombre));
            }

            if(!string.IsNullOrEmpty(request.InstructorRequest!.Apellido)){
                predicate = predicate.And(y => y.Apellidos!.Contains(request.InstructorRequest.Apellido));
            }

            if(!string.IsNullOrEmpty(request.InstructorRequest.OrderBy)){

                Expression<Func<Instructor,object>>? orderBySelecttor = 
                request.InstructorRequest.OrderBy.ToLower() switch
                {
                    "nombre" => instructor => instructor.Nombre!,
                    "apellido" => instructor => instructor.Apellidos!,
                    _ => instructor => instructor.Nombre!
                };

                bool orderby = request.InstructorRequest.OrderAsc ? request.InstructorRequest.OrderAsc :true;

                queryable = orderby ? queryable.OrderBy(orderBySelecttor) : queryable.OrderByDescending(orderBySelecttor);

            }

            queryable = queryable.Where(predicate);

            var instructorQuery = queryable.ProjectTo<InstructorResponse>(_mapper.ConfigurationProvider).AsQueryable();

            var pagination = await PagedList<InstructorResponse>.CreateAsync(instructorQuery,request.InstructorRequest.PageNumber,request.InstructorRequest.PageSize);

            return Result<PagedList<InstructorResponse>>.Success(pagination);
        }
    }
}
public record InstructorResponse(
    Guid? Id,
    string? Nombre,
    string? Apellido,
    string? Grado
)
{
    public InstructorResponse() : this(null, null, null, null)
    {
    }
}