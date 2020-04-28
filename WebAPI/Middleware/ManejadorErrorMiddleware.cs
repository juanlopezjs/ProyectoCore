using System.Net;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErrorMiddleware> _logger;
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger){
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context){
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ManejadorExcepcionesAsyncrono(context, ex, _logger);
            }
        }

        private async Task  ManejadorExcepcionesAsyncrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger){
            object errors = null;
            
            switch(ex){
                case ManejadorExcepcion me : 
                    logger.LogError(ex, "Manejador Error");
                    errors = me.Errors;
                    context.Response.StatusCode = (int)me.Code;
                    break;
                case Exception e:
                    logger.LogError(ex, "Error de servidor");
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error": e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if(errors != null){
                var resultados = JsonConvert.SerializeObject(new {errors});
                await context.Response.WriteAsync(resultados);
            }
        }
    }
}