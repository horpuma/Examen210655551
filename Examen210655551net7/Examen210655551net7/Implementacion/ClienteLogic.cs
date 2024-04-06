using Examen210655551net7.Contratos;
using Examen210655551net7.Modelos.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Examen210655551net7.Implementacion.ClienteLogic;

namespace Examen210655551net7.Implementacion
{
    internal class ClienteLogic : IClienteLogic
    {
        private readonly Contexto contexto;

        public ClienteLogic(Contexto contexto)
        {
            this.contexto = contexto;
        }
        public async Task<bool> Eliminar(int id)
        {
            var eliminar = await contexto.Clientes.FindAsync(id);
            if (eliminar == null)
                return false;

            contexto.Clientes.Remove(eliminar);
            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Cliente insertar)
        {
            bool sw = false;
            contexto.Clientes.Add(insertar);
            int response = await contexto.SaveChangesAsync();
            if (response == 1)
            {
                sw = true;
            }
            return sw;
        }

        public async Task<List<Cliente>> ListarTodos()
        {
            var lista = await contexto.Clientes.ToListAsync();
            return lista;
        }

        public async Task<bool> Modificar(Cliente cliente, int id)
        {
            var editar = await contexto.Clientes.FindAsync(id);
            if (editar == null)
                return false;

            editar.Nombre = cliente.Nombre;
            editar.Apellidos = cliente.Apellidos;

            await contexto.SaveChangesAsync();
            return true;
        }

        public async Task<Cliente> ObtenerById(int id)
        {
            var objeto = await contexto.Clientes.FindAsync(id);
            return objeto;
        }
    }
}

