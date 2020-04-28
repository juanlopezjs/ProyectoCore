using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest {
            public int CursoId {get; set;}
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly Context _context;

            public Manejador(Context context){
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if(curso  == null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el curso"});
                }

                _context.Remove(curso);
                var valor = await _context.SaveChangesAsync();

                return valor > 0 ? Unit.Value : throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "Error al eliminar el curso."});

            }
        }
    }
}