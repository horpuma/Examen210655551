using Examen210655551net7.Contratos;
using Examen210655551net7.Modelos.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Examen210655551net7.Endpoints
{
    public class ClienteFunction
    {
        private readonly ILogger<ClienteFunction> _logger;
        private readonly IClienteLogic clienteLogic;

        public ClienteFunction(ILogger<ClienteFunction> logger, IClienteLogic clienteLogic)
        {
            _logger = logger;
            this.clienteLogic = clienteLogic;
        }

        [Function("ListarClientes")]
        public async Task<HttpResponseData> ListarClientes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarclientes")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar clientes.");
            try
            {
                var listaClientes = clienteLogic.ListarTodos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaClientes.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        
        [Function("ListarClienteId")]
        public async Task<HttpResponseData> ListarClienteId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "listarClientes/{id:int}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("Ejecutando azure function para listar un Cliente por ID.");
            try
            {
                var ClienteId = clienteLogic.ObtenerById(id);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(ClienteId.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("InsertarCliente")]
        public async Task<HttpResponseData> InsertarCliente([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertarCliente")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar Clientes.");
            try
            {
                var afi = await req.ReadFromJsonAsync<Cliente>() ?? throw new Exception("Debe ingresar un Cliente con todos sus datos");
                bool seGuardo = await clienteLogic.Insertar(afi);
                if (seGuardo)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }

        [Function("ModificarCliente")]
        public async Task<HttpResponseData> ModificarCliente(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarCliente/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar Cliente con Id: {id}.");
            try
            {
                var Cliente = await req.ReadFromJsonAsync<Cliente>() ?? throw new Exception("Debe ingresar un Cliente con todos sus datos");
                bool modificado = await clienteLogic.Modificar(Cliente, id);

                if (modificado)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }

                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }


        [Function("EliminarCliente")]
        public async Task<HttpResponseData> EliminarCliente(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "eliminarCliente/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar Cliente con Id: {id}.");
            try
            {
                bool eliminado = await clienteLogic.Eliminar(id);

                if (eliminado)
                {
                    var respuesta = req.CreateResponse(HttpStatusCode.OK);
                    return respuesta;
                }

                return req.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }
        }
    }
}
