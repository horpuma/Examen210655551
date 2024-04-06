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
        //1. Realizar un endpoint para que registre un pedido y su respectivo detalle
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

        //2. Listar el siguiente reporte, nombrecliente, fechadelpedido ,nombrepedido
        public async Task<List<DtoClientePedido>> ListarClientePedido()
        {
            var lista = await contexto.Pedidos.Select(x=>new DtoClientePedido{
                NombreCLiente=x.IdClienteNavigation.Nombre,
                FechaPedido=x.Fecha
            }).ToListAsync();
            return lista;
        }

        //3. Realizar el siguiente reporte, Identificar los 3 productos mas pedidos o solicitados
        public async Task<List<DtoProductoVendido>> Listar3Productos()
        {
            var lista = await contexto.Detalles
            .GroupBy(det => det.IdProducto)
            .Select(g => new DtoProductoVendido
            {
                NombreProducto = g.FirstOrDefault().IdDetalleProductoNavigation.Nombre,
                cantidadTotalSum = g.Sum(det => det.Cantidad),
                cantidadTotalCount = g.Count()
            })
            .OrderByDescending(x => x.cantidadTotalCount)
            .Take(3)
            .ToListAsync();
            return lista;
        }

        //4. Realizar un enpoint para actualizar un pedido y su respectivo detalle
        public async Task<bool> ModificarPedidoDetalle(Pedido pedido, int id)
        {
            var editar = await contexto.Pedidos.FindAsync(id);
            var detalles = pedido.Detalles;
            if (editar == null)
                return false;

            editar.Fecha  = pedido.Fecha;
            editar.Total  = pedido.Total;
            editar.Estado =pedido.Estado;

            await contexto.SaveChangesAsync();
            if (detalles!= null)
            {
                foreach(var det in detalles)
                {
                    if (det.IdDetalle != null)
                    {
                        var editDet= await contexto.Detalles.FindAsync(det.IdDetalle);
                        if(editDet != null)
                        {
                            editDet.IdProducto = det.IdProducto;
                            editDet.Cantidad = det.Cantidad;
                            editDet.Precio = det.Precio;
                            editDet.SubTotal = det.SubTotal;
                            await contexto.SaveChangesAsync();
                        }
                    }
                }
            }

            return true;
        }

        //5. Realizar un endpoint para realizar un eliminado en cascada de la tabla cliente
        public async Task<bool> EliminarClienteCascada(int id)
        {
            var eliminar = await contexto.Clientes.FindAsync(id);
            if (eliminar == null)
                return false;

            contexto.Clientes.Remove(eliminar);
            await contexto.SaveChangesAsync();
            return true;
        }
        //6. Realizar un ednpoint que determine cuales son los productos mas vendidos segun un rango de fechas como parametros
        public async Task<List<DtoProductosVendidosFecha>> ListarProductosMasVendidosFecha(DateTime f1, DateTime f2)
        {
            var lista = await contexto.Detalles
                    .Where(d => d.IdDetallePedidoNavigation.Fecha >= f1 && d.IdDetallePedidoNavigation.Fecha <= f2)
                    .GroupBy(d => d.IdProducto)
                    .Select(g => new DtoProductosVendidosFecha 
                    {
                        NombreProducto = g.FirstOrDefault().IdDetalleProductoNavigation.Nombre,
                        cantidad = g.Count(),
                        fecha = g.FirstOrDefault().IdDetallePedidoNavigation.Fecha,
                    })
                    .OrderByDescending(x => x.cantidad)
                    .ToListAsync();
            return lista;
        }

        
    }
}
