using Examen210655551net7.Modelos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen210655551net7.Contratos
{
    public interface IClienteLogic
    {
        public Task<bool> Insertar(Cliente cliente);
        public Task<bool> Modificar(Cliente cliente, int id);
        public Task<bool> Eliminar(int id);
        public Task<List<Cliente>> ListarTodos();
        public Task<Cliente> ObtenerById(int id);
    }
}
