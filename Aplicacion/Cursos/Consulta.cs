using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDto>>{}

        public class Manejador : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            private readonly Context _context;
            private readonly IMapper _mapper;

            public Manejador(Context context, IMapper mapper){
                _context = context;
                _mapper = mapper;
            }

            public  async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso
                    .Include(x => x.Comentario)
                    .Include(x => x.Precio)
                    .Include(x => x.CursoInstructor)
                    .ThenInclude(x => x.Instructor)
                    .ToListAsync();

                return _mapper.Map<List<Curso>, List<CursoDto>>(curso);
            }
        }
    }
}