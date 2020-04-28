using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistencia;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly Context _context;
        public WeatherForecastController(Context context){
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Curso> Get(){
            return _context.Curso.ToList();
        }
    }
}
