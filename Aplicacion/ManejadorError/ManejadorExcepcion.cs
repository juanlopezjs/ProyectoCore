using System.Net;
using System;
namespace Aplicacion.ManejadorError
{
    public class ManejadorExcepcion : Exception
    {
        public HttpStatusCode Code {get;}
        public object Errors {get;}
        public ManejadorExcepcion(HttpStatusCode code, object errors = null){
            Code = code;
            Errors = errors;
        }
    }
}