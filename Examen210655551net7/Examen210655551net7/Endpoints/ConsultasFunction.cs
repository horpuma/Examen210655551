using Examen210655551net7.Contratos;
using Examen210655551net7.Implementacion;
using Examen210655551net7.Modelos.DTO;
using Examen210655551net7.Modelos.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Examen210655551net7.Endpoints
{
    public class ConsultasFunction
    {
        private readonly ILogger<ConsultasFunction> _logger;
        private readonly IConsultasLogic consultasLogic;

        public ConsultasFunction(ILogger<ConsultasFunction> logger, IConsultasLogic consultasLogic)
        {
            _logger = logger;
            this.consultasLogic = consultasLogic;
        }

        [Function("InsertarPedidoDetalle")]
        [OpenApiOperation("InsertarPedidoDetallespec", "InsertarPedidoDetalle", Description = " Sirve para ingresar un Pedido y Detalles")]
        [OpenApiRequestBody("application/json", typeof(Pedido), Description = "Ingresar Pedido y Detalles nuevos")]
        public async Task<HttpResponseData> InsertarCliente([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertarPedidoDetalle")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para insertar Clientes.");
            try
            {
                var ped = await req.ReadFromJsonAsync<Pedido>() ?? throw new Exception("Debe ingresar un Pedido con todos sus datos");
                //var Det = await req.ReadFromJsonAsync<List<Detalle>>() ?? throw new Exception("Debe ingresar LosDetalles con todos sus datos");

                bool seGuardo = await consultasLogic.InsertarPedidoDetalles(ped);
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
        [Function("ListarClientePedido")]
        [OpenApiOperation("ListarClientePedidospec", "ListarClientePedido", Description = " Sirve para listar clientes y sus pedidos")]

        public async Task<HttpResponseData> ListarClientes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarClientePedido")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar clientes ys sus pedidos.");
            try
            {
                var listaClientes = consultasLogic.ListarClientePedido();
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
    }
}
