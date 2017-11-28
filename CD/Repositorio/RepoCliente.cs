using CD.ModeloBase;
using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD.Repositorio
{
    public class RepoCliente
    {
        public ClienteDTO BuscarCliente(int cedula)
        {
            /*Busca los clientes esten activos o no*/

            //Instanciamos la base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos el cliente
                var unCliente = miBase.Cliente.Where(c => c.cedula == cedula && c.habilitado == true).FirstOrDefault();

                //Vemos si NO encontramos un cliente
                if (unCliente == null)
                {
                    return null;
                }
                else //Si efectivamente encontramos un cliente
                {
                    //Creamos el cliente Resultante
                    ClienteDTO resultado = new ClienteDTO
                    {
                        Cedula = unCliente.cedula,
                        Nombre = unCliente.nombre,
                        Domicilio = unCliente.domicilio,
                        FechaNacimiento = unCliente.fechaNacimiento,
                        NickUsuario = unCliente.nickUsuario,
                        Habilitado = unCliente.habilitado
                    };

                    //Devolvemos el cliente resultado
                    return resultado;
                }
            }
        }

        public void CrearCliente(ClienteDTO nuevoCliente)
        {
            //Instanciamos nuestra Base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Pasamos nuestro ClienteDTO a Cliente
                Cliente miCliente = new Cliente()
                {
                    cedula = nuevoCliente.Cedula,
                    nombre = nuevoCliente.Nombre,
                    domicilio = nuevoCliente.Domicilio,
                    fechaNacimiento = nuevoCliente.FechaNacimiento,
                    nickUsuario = nuevoCliente.NickUsuario,
                    //Como lo estamos creando, esta habilitado
                    habilitado = true
                };

                //Agregamos el cliente a nuestra base
                miBase.Cliente.Add(miCliente);
                //Guardamos los cambios
                miBase.SaveChanges();
            }
        }

        public void HabilitarCliente(int Cliente)
        {
            /*Reactivamos el cliente deshabilitado, es muy similar a editar*/

            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos nuestro cliente en la base con una consulta lambda
                var miCliente = miBase.Cliente.Where(c => c.cedula == Cliente).First();
                //Fijamos el cliente
                miBase.Cliente.Attach(miCliente);
                //Dejamos todos los campos iguales salvo habilitado que pasa a ser true
                miCliente.habilitado = true;
                //Guardamos los cambios de la base
                miBase.SaveChanges();
            }
        }

        public void EditarCliente(ClienteDTO Cliente)
        {
            /*Solo se pueden editar los clientes habilitados*/

            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos nuestro cliente en la base de datos
                var miCliente = (from unCliente in miBase.Cliente where unCliente.cedula == Cliente.Cedula select unCliente).First();

                //Fijamos el cliente
                miBase.Cliente.Attach(miCliente);

                //Remapeamos sus datos
                miCliente.cedula = Cliente.Cedula;
                miCliente.domicilio = Cliente.Domicilio;
                miCliente.fechaNacimiento = Cliente.FechaNacimiento;
                miCliente.nickUsuario = Cliente.NickUsuario;
                miCliente.nombre = Cliente.Nombre;
                //Solo editamos los clientes habilitados, por tanto el cliente actual.habilitado = true
                miCliente.habilitado = true;

                //Guardamos los cambios que acabamos de hacer
                miBase.SaveChanges();
            }
        }

        public List<ClienteDTO> ListaClientes()
        {
            /*Devuelve una lista de ClienteDTO esten habilitados o no*/

            //Instanciamos la base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Haciendo una consulta linQ creamos una lista con todos los clientes existentes
                var lista = (from unCliente in miBase.Cliente
                             select new ClienteDTO()
                             {
                                 Cedula = unCliente.cedula,
                                 Nombre = unCliente.nombre,
                                 Domicilio = unCliente.domicilio,
                                 FechaNacimiento = unCliente.fechaNacimiento,
                                 NickUsuario = unCliente.nickUsuario,
                                 Habilitado = unCliente.habilitado
                             });

                //Si la lista es nula retornamos null, sino retornamos la lista
                if (lista != null)
                {
                    return lista.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public void EliminarCliente(int Cliente)
        {
            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos nuestro cliente en la base de datos
                var miCliente = (from unCliente in miBase.Cliente where unCliente.cedula == Cliente select unCliente).First();

                //Fijamos el cliente
                miBase.Cliente.Attach(miCliente);

                //dejamos todo igual, solo cambiamos habilitado
                miCliente.cedula = miCliente.cedula;
                miCliente.domicilio = miCliente.domicilio;
                miCliente.fechaNacimiento = miCliente.fechaNacimiento;
                miCliente.nickUsuario = miCliente.nickUsuario;
                miCliente.nombre = miCliente.nombre;
                //Deshabilitamos el cliente
                miCliente.habilitado = false;

                //Guardamos los cambios que acabamos de hacer
                miBase.SaveChanges();
            }
        }

        public List<ClienteDTO> ClientesUsuario(string usuario)
        {
            /*Devolveremos la lista de clientes que creo cierto usuario*/

            //Instanciamos la base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Haciendo una consulta linQ creamos una lista con todos los clientes existentes
                var lista = (from unCliente in miBase.Cliente where unCliente.nickUsuario.Trim().ToLower() == usuario.Trim().ToLower()
                             select new ClienteDTO()
                             {
                                 Cedula = unCliente.cedula,
                                 Nombre = unCliente.nombre,
                                 Domicilio = unCliente.domicilio,
                                 FechaNacimiento = unCliente.fechaNacimiento,
                                 NickUsuario = unCliente.nickUsuario,
                                 Habilitado = unCliente.habilitado
                             });

                //Retornamos la lista de clientes creados por el usuario
                return lista.ToList();
            }
        }

        /*DATOS*/
        public int TotalClientesRegistrados()
        {
            //Instanciamos nuestra base de datos
            using(BaseSistema miBase = new BaseSistema())
            {
                //Consultamos la cantidad de clientes registrados
                var res = miBase.Cliente.Count();

                //Devolvemos esa cantidad
                return res;
            }
        }
    }
}
