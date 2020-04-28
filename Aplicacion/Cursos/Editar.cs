using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest {
            public int CursoId {get; set;}
            public string Titulo {get; set;}
            public string Descripcion {get; set;}
            public DateTime? FechaPublicacion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("Debe ingresar el titulo");
                RuleFor(x => x.Descripcion).NotEmpty().WithMessage("Debe ingresar la descripción");
                RuleFor(x => x.FechaPublicacion).NotEmpty().WithMessage("Debe ingresar la fecha de publicacion");
            }
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

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                var valor = await _context.SaveChangesAsync();

                return valor > 0 ? Unit.Value : throw new Exception("Error al editar el curso.");
            }
        }
    }
}