using CL.Logicas;
using EC;
using FacturacionBOOSTRAP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturacionBOOSTRAP.HerramientasModel
{
    public static class HerramientasM
    {
        /*
         * Estas herramientas seran funciones que controlen datos que la logica no puede controlar
         * Por ejemplo, sessiones, devuelve modelos, Convierte de DTO a MODEL, etc...
         */

        /* ******************************************************
        *            SECCION HERRAMIENTAS DE FACTURA
        * ******************************************************/
        public static FacturaModel ConvertirAFacturaModel(FacturaDTO miFactura)
        {
            //Cargamos nombre y marca del producto y el nombre del cliente
            FacturaModel unaFacturaModel = new FacturaModel()
            {
                Fecha = miFactura.Fecha,
                NickUsuario = miFactura.NickUsuario,
                Total = miFactura.Total,
                NumeroCliente = miFactura.NumeroCliente,
                NumeroFactura = miFactura.numeroFactura,
                NombreCliente = NombreCliente(miFactura.NumeroCliente),
                ListaDetalle = new List<DetalleFacturaModel>()
            };

            //Creamos la lista de detalles vacia
            List<DetalleFacturaModel> lista = new List<DetalleFacturaModel>();

            //Convertimos cada detalle DTO en detalle model agregandole Marca y nombreCliente para que se vea bien en la vista detalles
            foreach (DetalleFacturaDTO f in miFactura.ListaDetalle)
            {
                //Creamos el detalle model
                DetalleFacturaModel unDetalle = new DetalleFacturaModel()
                {
                    Cantidad = f.Cantidad,
                    IdProducto = f.IdProducto,
                    SubTotal = f.SubTotal,
                    //Cargamos nombre y marca para que se muestre en la factura
                    NombreMarca = NombreMarcaProducto(f.IdProducto)
                };
                //Agregamos el detalle a la lista de detalles
                lista.Add(unDetalle);
            }

            //Agregamos la lista a la factura
            unaFacturaModel.ListaDetalle = lista;

            //Retornamos la Factura
            return unaFacturaModel;
        }

        public static List<FacturaModel> ListaFacturaModel(List<FacturaDTO> unaLista)
        {
            /*Este metodo convierte una lista de FacturaDTO en FacturaModel*/

            //Creamos la lista resultado
            List<FacturaModel> resultado = new List<FacturaModel>();

            //Para cada FacturaDTO
            foreach (FacturaDTO unaFactura in unaLista)
            {
                //Creamos una FacturaModel y le cargamos los datos
                FacturaModel miFactura = new FacturaModel()
                {
                    NumeroFactura = unaFactura.numeroFactura,
                    NumeroCliente = unaFactura.NumeroCliente,
                    Fecha = unaFactura.Fecha,
                    Total = unaFactura.Total,
                    NickUsuario = unaFactura.NickUsuario,
                    //Mostramos a la hora del detalle o del listado el nombre del cliente
                    NombreCliente = NombreCliente(unaFactura.NumeroCliente),
                    ListaDetalle = new List<DetalleFacturaModel>()
                };

                //Ahora le cargamos sus detalles correspondientes
                foreach(DetalleFacturaDTO d in unaFactura.ListaDetalle)
                {
                    //Creamos un detalleModel
                    DetalleFacturaModel unDetalle = new DetalleFacturaModel()
                    {
                        Cantidad = d.Cantidad,
                        IdProducto = d.IdProducto,
                        //mostramos a la hora del detalle la descripcion del producto
                        NombreMarca = NombreMarcaProducto(d.IdProducto),
                        SubTotal = d.SubTotal
                    };

                    //Agregamos el detalle a la factura
                    miFactura.ListaDetalle.Add(unDetalle);
                }

                //Agregamos la factura a la lista resultado
                resultado.Add(miFactura);
            }

            //Retornamos la lista resultado
            return resultado;
        }

        //Devolvemos marca y nombre del producto
        private static string NombreMarcaProducto(int idProducto)
        {
            //Anticipamos un posible error en la base
            try
            {
                //Instanciamos la logica de productos
                LogicaProducto logicaProducto = new LogicaProducto();

                //Buscamos el producto
                ProductoDTO unProducto = logicaProducto.BuscarProducto(idProducto);

                //Juntamos el nombre y la marca en un mismo string
                string resultado = unProducto.Nombre + " " + unProducto.Marca;

                //Retornamos nombre y marca
                return resultado;
            }
            catch (Exception)
            {
                //Retornamos un error para saber que algo fallo
                return ("Algo salio mal");
            }
        }

        //Devolveremos el nombre del cliente
        private static string NombreCliente(int idCliente)
        {
            //Anticipamos un posible error en la base
            try
            {
                //Instanciamos la logica de Clientes
                LogicaCliente logicaCliente = new LogicaCliente();

                //Buscamos el Cliente
                ClienteDTO unCliente = logicaCliente.BuscarCliente(idCliente);

                //retornamos el nombre de cliente

                return unCliente.Nombre;
            }
            catch (Exception)
            {
                //Retornamos un error para saber que algo fallo
                return ("Algo salio mal");
            }
        }



        /* ******************************************************
         *            SECCION HERRAMIENTAS DE USUARIO
         * ******************************************************/



        public static string IdLogueado(UsuarioModel usuarioActual)
        {
            /*Precondicion - La sesion no es nula, es decir, usuarioActual no viene nulo*/
            return usuarioActual.Usuario;
        }

        public static bool EsAdmin(UsuarioModel usuarioActual)
        {
            /*Precondicion - La sesion no es nula, es decir, usuarioActual no viene nulo*/
            return usuarioActual.EsAdmin;
        }

        public static List<UsuarioModel> UsuariosHabilitados(List<UsuarioDTO> unaLista)
        {
            /*Recibimos una lista de usuarios y devolvemos solo los usuarios habilitados*/

            //Creamos la lista resultante
            List<UsuarioModel> resultado = new List<UsuarioModel>();

            foreach (UsuarioDTO unUsuario in unaLista)
            {
                //Si el usuario esta habilitado lo agregamos a la lista resultado
                if (unUsuario.Habilitado)
                {
                    //Creamos el usuario
                    UsuarioModel miUsuario = new UsuarioModel()
                    {
                        Usuario = unUsuario.Usuario,
                        EsAdmin = unUsuario.EsAdmin,
                        Contrasena = unUsuario.Contrasena
                    };

                    //Agregamos el usuario a la lista
                    resultado.Add(miUsuario);
                }
            }
            //devolvemos la lista resultante
            return resultado;
        }

        public static List<UsuarioModel> UsuariosDeshabilitados(List<UsuarioDTO> unaLista)
        {
            /*Recibimos una lista de usuarios y devolvemos solo los usuarios deshabilitados*/

            //Creamos la lista resultante
            List<UsuarioModel> resultado = new List<UsuarioModel>();

            foreach (UsuarioDTO unUsuario in unaLista)
            {
                //Si el usuario NO esta habilitado lo agregamos a la lista resultado
                if (!unUsuario.Habilitado)
                {
                    //Creamos el usuario
                    UsuarioModel miUsuario = new UsuarioModel()
                    {
                        Usuario = unUsuario.Usuario,
                        EsAdmin = unUsuario.EsAdmin,
                        Contrasena = unUsuario.Contrasena
                    };

                    //Agregamos el usuario a la lista
                    resultado.Add(miUsuario);
                }
            }
            //devolvemos la lista resultante
            return resultado;
        }

        public static UsuarioModel ConvertirUsuarioDTO(UsuarioDTO unUsuario)
        {
            /*Este metodo toma un usuarioDTO y lo convierte a UsuarioModel*/

            //Si viene null retornamos null
            if (unUsuario == null)
            {
                return null;
            }
            else //Si no viene null
            {
                //Creamos el usuario resultante
                UsuarioModel resultado = new UsuarioModel()
                {
                    Usuario = unUsuario.Usuario,
                    EsAdmin = unUsuario.EsAdmin,
                    Contrasena = unUsuario.Contrasena
                };
                //Devolvemos el usuario convertido a UsuarioModel
                return resultado;
            }
        }

        public static UsuarioDTO ConvertirUsuarioModel(UsuarioModel unUsuario)
        {
            /*Este metodo toma un usuarioModel y lo convierte a UsuarioDTO*/

            //Si viene null retornamos null
            if (unUsuario == null)
            {
                return null;
            }
            else //Si no viene null
            {
                //Creamos el usuario resultante
                UsuarioDTO resultado = new UsuarioDTO()
                {
                    Usuario = unUsuario.Usuario,
                    EsAdmin = unUsuario.EsAdmin,
                    Contrasena = unUsuario.Contrasena
                };
                //Devolvemos el usuario convertido a UsuarioDTO
                return resultado;
            }
        }

        /* ******************************************************
        *            SECCION HERRAMIENTAS DE PRODCTO
        * ******************************************************/


        public static ProductoModel ConvertirProductoDTO(ProductoDTO unProducto)
        {
            /*Este metodo toma un ProductoDTO y lo convierte a ProductoModel*/

            //Si viene null retornamos null
            if (unProducto == null)
            {
                return null;
            }
            else //Si no viene null
            {
                //Creamos el Producto resultante
                ProductoModel resultado = new ProductoModel()
                {
                    Id = unProducto.Id,
                    Nombre = unProducto.Nombre,
                    Marca = unProducto.Marca,
                    NickUsuario = unProducto.NickUsuario,
                    Precio = unProducto.Precio
                };
                //Devolvemos el producto convertido a ProductoModel
                return resultado;
            }
        }

        public static List<ProductoModel> ProductosHabilitados(List<ProductoDTO> unaLista)
        {
            //Creamos la lista resultante
            List<ProductoModel> resultado = new List<ProductoModel>();

            //Para cada elemento habilitado de la lista que nos llega creamos un clientemodel y lo agregamos a la lista resultado
            foreach (ProductoDTO unProducto in unaLista)
            {
                //Vemos si esta habilitado
                if (unProducto.Habilitado)
                {
                    //Creamos el productoModel
                    ProductoModel miProducto = new ProductoModel()
                    {
                        Id = unProducto.Id,
                        Marca = unProducto.Marca,
                        NickUsuario = unProducto.NickUsuario,
                        Nombre = unProducto.Nombre,
                        Precio = unProducto.Precio
                    };

                    //Agregamos el producto a la lista
                    resultado.Add(miProducto);
                }
            }

            //Retornamos la lista que seara null o no, segun si hay o no productos habilitados
            return resultado;
        }

        public static List<ProductoModel> ProductosDeshabilitados(List<ProductoDTO> unaLista)
        {
            //Creamos la lista resultante
            List<ProductoModel> resultado = new List<ProductoModel>();

            //Para cada elemento deshabilitado de la lista que nos llega creamos un clientemodel y lo agregamos a la lista resultado
            foreach (ProductoDTO unProducto in unaLista)
            {
                //Vemos si esta deshabilitado
                if (!unProducto.Habilitado)
                {
                    //Creamos el productoModel
                    ProductoModel miProducto = new ProductoModel()
                    {
                        Id = unProducto.Id,
                        Marca = unProducto.Marca,
                        NickUsuario = unProducto.NickUsuario,
                        Nombre = unProducto.Nombre,
                        Precio = unProducto.Precio
                    };

                    //Agregamos el producto a la lista
                    resultado.Add(miProducto);
                }
            }

            //Retornamos la lista que seara null o no, segun si hay o no productos deshabilitados
            return resultado;
        }

        public static ProductoDTO ConvertirProductoModel(ProductoModel unProducto)
        {
            /*Este metodo toma un ProductoModel y lo convierte a ProductoDTO*/

            //Si viene null retornamos null
            if (unProducto == null)
            {
                return null;
            }
            else //Si no viene null
            {
                //Creamos el Producto resultante
                ProductoDTO resultado = new ProductoDTO()
                {
                    Id = unProducto.Id,
                    Nombre = unProducto.Nombre,
                    Marca = unProducto.Marca,
                    NickUsuario = unProducto.NickUsuario,
                    Precio = unProducto.Precio
                };
                //Devolvemos el Producto convertido a ProductoDTO
                return resultado;
            }
        }


        /* ******************************************************
        *            SECCION HERRAMIENTAS DE CLIENTE
        * ******************************************************/

        public static ClienteModel ConvertirClienteDTO(ClienteDTO unCliente)
        {
            /*Este metodo toma un ClienteDTO y lo convierte a ClienteModel*/

            //Si viene null retornamos null
            if (unCliente == null)
            {
                return null;
            }
            else //Si no viene null
            {
                //Creamos el Producto resultante
                ClienteModel resultado = new ClienteModel()
                {
                    Cedula = unCliente.Cedula,
                    Domicilio = unCliente.Domicilio,
                    FechaNacimiento = unCliente.FechaNacimiento,
                    NickUsuario = unCliente.NickUsuario,
                    Nombre = unCliente.Nombre
                };
                //Devolvemos el cliente convertido a ClienteModel
                return resultado;
            }
        }

        public static ClienteDTO ConvertirClienteModel(ClienteModel unCliente)
        {
            /*Este metodo toma un ClienteModel y lo convierte a ClienteDTO*/

            //Si viene null retornamos null
            if (unCliente == null)
            {
                return null;
            }
            else //Si no viene null
            {
                //Creamos el Producto resultante
                ClienteDTO resultado = new ClienteDTO()
                {
                    Cedula = unCliente.Cedula,
                    Domicilio = unCliente.Domicilio,
                    FechaNacimiento = unCliente.FechaNacimiento,
                    NickUsuario = unCliente.NickUsuario,
                    Nombre = unCliente.Nombre
                };
                //Devolvemos el cliente convertido a ClienteDTO
                return resultado;
            }

        }

        public static List<ClienteModel> ClientesDeshabilitados(List<ClienteDTO> unaLista)
        {
            //Creamos la lista resultante
            List<ClienteModel> resultado = new List<ClienteModel>();

            //Para cada cliente deshabilitado de la lista que nos llega creamos un ClienteModel y lo agregamos a la lista resultado
            foreach (ClienteDTO unCliente in unaLista)
            {
                //si el cliente esta deshabilitado
                if (!unCliente.Habilitado)
                {
                    //Creamos un clienteModel con los datos correspondientes
                    ClienteModel miCliente = new ClienteModel()
                    {
                        Cedula = unCliente.Cedula,
                        Domicilio = unCliente.Domicilio,
                        Nombre = unCliente.Nombre,
                        FechaNacimiento = unCliente.FechaNacimiento,
                        NickUsuario = unCliente.NickUsuario
                    };
                    //Agregamos el cliente a la lista
                    resultado.Add(miCliente);
                }
            }

            //Devolvemos la lista resultado que puede ser null, dependiendo de si hay o no clientes deshabilitados
            return resultado;
        }

        public static List<ClienteModel> ClientesHabilitados(List<ClienteDTO> unaLista)
        {
            //Creamos la lista resultante
            List<ClienteModel> resultado = new List<ClienteModel>();

            //Para cada cliente habilitado de la lista que nos llega creamos un ClienteModel y lo agregamos a la lista resultado
            foreach (ClienteDTO unCliente in unaLista)
            {
                //si el cliente esta habilitado
                if (unCliente.Habilitado)
                {
                    //Creamos un clienteModel con los datos correspondientes
                    ClienteModel miCliente = new ClienteModel()
                    {
                        Cedula = unCliente.Cedula,
                        Domicilio = unCliente.Domicilio,
                        Nombre = unCliente.Nombre,
                        FechaNacimiento = unCliente.FechaNacimiento,
                        NickUsuario = unCliente.NickUsuario
                    };
                    //Agregamos el cliente a la lista
                    resultado.Add(miCliente);
                }
            }

            //Devolvemos la lista resultado que puede ser null, dependiendo de si hay o no clientes habilitados
            return resultado;
        }




    }
}