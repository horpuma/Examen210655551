using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen210655551net7.Modelos.Entity
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }

        [InverseProperty("IdClienteNavigation")]
        public virtual ICollection<Pedido>? Pedidos { get; set; } = new List<Pedido>();
    }
}
