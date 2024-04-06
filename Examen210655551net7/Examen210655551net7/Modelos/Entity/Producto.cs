using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen210655551net7.Modelos.Entity
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }

        [InverseProperty("IdDetalleProductoNavigation")]
        public virtual ICollection<Detalle>? Detalles { get; set; } = new List<Detalle>();
    }
}
