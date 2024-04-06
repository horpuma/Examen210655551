using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen210655551net7.Modelos.Entity
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string? Estado { get; set; }
        [ForeignKey("IdCliente")]
        public virtual Cliente? IdClienteNavigation { get; set; }
        [InverseProperty("IdDetallePedidoNavigation")]
        public virtual ICollection<Detalle>? Detalles { get; set; } = new List<Detalle>();

    }
}
