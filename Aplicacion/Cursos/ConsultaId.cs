using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<Curso> {
            
            
            public int Id {get; set;}
        }

        public class Manejador : IRequestHandler<CursoUnico, Curso>
        {
            private readonly Context _context;
            public Manejador(Context context){
                _context = context;
            }

            public async Task<Curso> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                return await _context.Curso.FindAsync(request.Id);
            }
        }
    }
}