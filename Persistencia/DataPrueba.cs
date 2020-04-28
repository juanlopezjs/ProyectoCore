using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarData(Context context, UserManager<Usuario> usuarioManager){
            if(!usuarioManager.Users.Any()){
                var usuario = new Usuario{NombreCompleto = "Juan López", UserName = "jmlopez", Email = "juan.lopez@imbanaco.com.co"};
                await usuarioManager.CreateAsync(usuario, "Pass1234.");
            }
        }
    }
}