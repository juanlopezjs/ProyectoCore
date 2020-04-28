using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBase
    {
    
        [HttpGet]
        public async Task<ActionResult<List<Curso>>> Get(){
            try
            {
                return await Mediator.Send(new Consulta.ListaCursos());
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> Detalle(int id)
        {
            try
            {
                return await Mediator.Send(new ConsultaId.CursoUnico{Id = id});
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Post(Nuevo.Ejecuta curso)
        {
            try
            {
                return await Mediator.Send(curso);
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Put(int id, Editar.Ejecuta curso)
        {
            try
            {
                curso.CursoId = id;
                return await Mediator.Send(curso);
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            try
            {
                return await Mediator.Send(new Eliminar.Ejecuta{CursoId = id});
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }
    }
}