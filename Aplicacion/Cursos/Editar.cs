using System;
using System.Collections.Generic;
using System.Linq;
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
            public Guid CursoId {get; set;}
            public string Titulo {get; set;}
            public string Descripcion {get; set;}
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor {get ; set;}
            public decimal? Precio {get; set;}
            public decimal? Promocion {get; set;}
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

                if(request.ListaInstructor != null && request.ListaInstructor.Count > 0){
                    var instructores = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                    _context.CursoInstructor.RemoveRange(instructores);

                    foreach(var ids in request.ListaInstructor){
                        var nuevoInstructor = new CursoInstructor {
                            CursoId = request.CursoId,
                            InstructorId = ids
                        };
                        _context.CursoInstructor.Add(nuevoInstructor);
                    }

                }

                var precio = _context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if(precio != null){
                    precio.Promocion = request.Promocion ?? precio.Promocion;
                    precio.PrecioActual = request.Precio ?? precio.PrecioActual;
                    _context.Precio.Update(precio);
                }else{
                    precio = new Precio {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId
                    };
                    await _context.Precio.AddAsync(precio);
                }
                var valor = await _context.SaveChangesAsync();

                return valor > 0 ? Unit.Value : throw new Exception("Error al editar el curso.");
            }
        }
    }
}