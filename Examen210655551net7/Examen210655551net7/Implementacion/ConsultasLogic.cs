using Examen210655551net7.Contratos;
using Examen210655551net7.Modelos.DTO;
using Examen210655551net7.Modelos.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen210655551net7.Implementacion
{
    internal class ConsultasLogic : IConsultasLogic
    {
        private readonly Contexto contexto;

        public ConsultasLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> InsertarPedidoDetalles(Pedido pedido)
        {
            bool sw = false;
            contexto.Pedidos.Add(pedido);
            int response=await contexto.SaveChangesAsync();
            /*foreach (Detalle de in detalles)
            {
                contexto.Detalles.Add(de);
            }
            int response = await contexto.SaveChangesAsync();*/
            if (response >= 1)
            {
                sw = true;
            }
            return sw;
        }
        public async Task<List<DtoClientePedido>> ListarClientePedido()
        {
            var lista = await contexto.Pedidos.Select(x=>new DtoClientePedido{
                NombreCLiente=x.IdClienteNavigation.Nombre,
                FechaPedido=x.Fecha
            }).ToListAsync();
            return lista;
        }
        public async Task<List<Producto>> Listar3Productos()
        {
            var lista = await contexto.Detalles.ToListAsync(x=> new Producto { });
            return lista;
        }
        public async Task<bool> ModificarPedidoDetalle(Pedido pedido, List<Detalle> detalles)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> EliminarClienteCascada(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Producto>> ListarProductosMasVendidos(DateTime f1, DateTime f2)
        {
            throw new NotImplementedException();
        }

        
    }
}
