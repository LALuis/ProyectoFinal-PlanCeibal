using CD.Repositorio;
using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Logicas
{
    public class LogicaCliente
    {
        //Instanciamos el repositorio de Cliente
        RepoCliente repoCliente = new RepoCliente();

        /*Buscamos y retornamos el cliente que nos pasan su id por parametro*/
        public ClienteDTO BuscarCliente(int cedula)
        {
            return repoCliente.BuscarCliente(cedula);
        }

        /*Registramos el cliente que nos pasan como parametro*/
        public void CrearCliente(ClienteDTO nuevoCliente)
        {
            repoCliente.CrearCliente(nuevoCliente);
        }

        /*Editamos el cliente cuyos nuevos datos(Sin tocar la cedula y datos no editables) nos llegan como parametro*/
        public void EditarCliente(ClienteDTO Cliente)
        {
            repoCliente.EditarCliente(Cliente);
        }

        /*Retornamos la lista de TODOS los clientes en el sistema*/
        public List<ClienteDTO> ListaClientes()
        {
            return repoCliente.ListaClientes();
        }

        /*Eliminamos el cliente cuyo id nos pasan como parametro*/
        public void EliminarCliente(int Cliente)
        {
            repoCliente.EliminarCliente(Cliente);
        }

        /*Habilitamos el cliente cuyo id nos pasan como parametro*/
        public void HabilitarCliente(int Cliente)
        {
            repoCliente.HabilitarCliente(Cliente);
        }

        /*HISTORIAL*/

        //Devuelve la lista de clientes creados por un usuario en concreto
        public List<ClienteDTO> ClientesUsuario(string usuario)
        {
            return repoCliente.ClientesUsuario(usuario);
        }

        /*DATOS*/
        public int TotalClientesRegistrados()
        {
            return repoCliente.TotalClientesRegistrados();
        }
    }
}
