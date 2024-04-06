using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Examen210655551net7.Modelos.Entity
{
    public class Detalle
    {
        [Key]
        public int IdDetalle { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal SubTotal { get; set; }
        [ForeignKey("IdPedido")]
        public virtual Pedido? IdDetallePedidoNavigation { get; set; }
        [ForeignKey("IdProducto")]
        public virtual Producto? IdDetalleProductoNavigation { get; set; }
    }
}
