using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class UsuarioController: MiControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login(Login.Ejecuta login){
            return await Mediator.Send(login);
        }
    }
}