using Examen210655551net7.Contratos;
using Examen210655551net7.Implementacion;
using Examen210655551net7.Modelos.DTO;
using Examen210655551net7.Modelos.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
                var listaClientePedido = consultasLogic.ListarClientePedido();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaClientePedido.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("ListarProductoTop3")]
        [OpenApiOperation("ListarProductoTop3spec", "ListarProductoTop3", Description = " Sirve para listar top 3 productos")]

        public async Task<HttpResponseData> Listar3Productos([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ListarProductoTop3")] HttpRequestData req)
        {
            _logger.LogInformation("Ejecutando azure function para listar top 3 productos");
            try
            {
                var listaProductos3 = consultasLogic.Listar3Productos();
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaProductos3.Result);
                return respuesta;
            }
            catch (Exception e)
            {
                var error = req.CreateResponse(HttpStatusCode.InternalServerError);
                await error.WriteAsJsonAsync(e.Message);
                return error;
            }

        }
        [Function("ModificarPedidoDetalle")]
        [OpenApiOperation("ModificarPedidoDetallespec", "ModificarPedidoDetalle")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del Pedido", Description = "ModificarPedidoDetalle", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody("application/json", typeof(Pedido), Description = "Ingresar Pedido y Detalles a modificar")]
        public async Task<HttpResponseData> ModificarCliente(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "modificarPedidoDetalle/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para modificar Pedido y sus Detalles de pedido: {id}.");
            try
            {
                var PedidosBody = await req.ReadFromJsonAsync<Pedido>() ?? throw new Exception("Debe ingresar un Cliente con todos sus datos");
                bool modificado = await consultasLogic.ModificarPedidoDetalle(PedidosBody, id);

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

        [Function("EliminarClienteCascada")]
        [OpenApiOperation("EliminarClienteEnCascadaspec", "EliminarClienteEnCascada")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Summary = "ID del cliente", Description = "El ID del cliente a eliminar en cascada", Visibility = OpenApiVisibilityType.Important)]
        public async Task<HttpResponseData> EliminarCliente(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "EliminarClienteCascada/{id}")] HttpRequestData req,
        int id)
        {
            _logger.LogInformation($"Ejecutando azure function para eliminar Cliente con Id: {id}.");
            try
            {
                bool eliminado = await consultasLogic.EliminarClienteCascada(id);

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
        [Function("ListarProductosMasPedidosFecha")]
        [OpenApiOperation("ListarProductosMasPedidosFechaspec", "ListarProductosMasPedidosFecha")]
        [OpenApiParameter(name: "f1", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "Fecha de inicio", Description = "Fecha de inicio del rango de fechas")]
        [OpenApiParameter(name: "f2", In = ParameterLocation.Query, Required = true, Type = typeof(DateTime), Summary = "Fecha de fin", Description = "Fecha de fin del rango de fechas")]
        public async Task<HttpResponseData> ListarProductosMasPedidosFecha(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ListarProductosMasPedidosFecha")] HttpRequestData req,
            DateTime f1,
            DateTime f2)
        {
            _logger.LogInformation("Ejecutando azure function para listar top productos vendidos entre 2 fechas");
            try
            {
                var listaProductos = consultasLogic.ListarProductosMasVendidosFecha(f1,f2);
                var respuesta = req.CreateResponse(HttpStatusCode.OK);
                await respuesta.WriteAsJsonAsync(listaProductos.Result);
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
