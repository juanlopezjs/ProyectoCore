using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBase
    {
    
        [HttpGet]
        public async Task<ActionResult<List<CursoDto>>> Get(){
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
        public async Task<ActionResult<CursoDto>> Detalle(Guid id)
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
        public async Task<ActionResult<Unit>> Put(Guid id, Editar.Ejecuta curso)
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
        public async Task<ActionResult<Unit>> Delete(Guid id)
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