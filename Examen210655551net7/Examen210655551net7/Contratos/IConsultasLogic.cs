using Examen210655551net7.Modelos.DTO;
using Examen210655551net7.Modelos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen210655551net7.Contratos
{
    public interface IConsultasLogic
    {
        public Task<bool> InsertarPedidoDetalles(Pedido pedido);
        public Task<List<DtoClientePedido>> ListarClientePedido();
        public Task<List<DtoProductoVendido>> Listar3Productos();
        public Task<bool> ModificarPedidoDetalle(Pedido pedido, int id);
        public Task<bool> EliminarClienteCascada(int id);
        public Task<List<DtoProductosVendidosFecha>> ListarProductosMasVendidosFecha(DateTime f1,DateTime f2);

    }
}
