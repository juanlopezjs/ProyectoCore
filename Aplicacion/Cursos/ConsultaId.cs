using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto> {
            
            
            public Guid Id {get; set;}
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            private readonly Context _context;
            private readonly IMapper _mapper;
            public Manejador(Context context, IMapper mapper){
                _context = context;
                _mapper = mapper;
            }

            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso
                .Include(x => x.Comentario)
                .Include(x => x.Precio)
                .Include(x => x.CursoInstructor)
                .ThenInclude(x => x.Instructor)
                .FirstOrDefaultAsync(a => a.CursoId == request.Id);

                return _mapper.Map<Curso, CursoDto>(curso);
            }
        }
    }
}