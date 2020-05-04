using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest {
            public string Titulo {get; set;}
            public string Descripcion {get; set;}
            public DateTime FechaPublicacion {get; set;}
            //public byte[] FotoPortada {get; set;}

            public List<Guid> ListaInstructor {get; set;}
            public decimal Precio {get; set;}
            public decimal Promocion {get; set;}
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
                var curso = new Curso{
                    CursoId = Guid.NewGuid(),
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };
                
                 await _context.Curso.AddAsync(curso);

                if(request.ListaInstructor != null){
                    foreach(var id in request.ListaInstructor){
                        var cursoInstructor = new CursoInstructor{
                            CursoId = curso.CursoId,
                            InstructorId = id
                        };
                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                var precio = new Precio {
                    CursoId = curso.CursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid()
                };
                
                _context.Precio.Add(precio);

                 var valor = await _context.SaveChangesAsync();

                 return valor > 0 ? Unit.Value : throw new Exception("Error al insertar el curso.");
            }
        }
    }
}