using CD.ModeloBase;
using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD.Repositorio
{
    public class RepoProducto
    {
        public ProductoDTO BuscarProducto(int idProducto)
        {
            /*Devolvera un productoDTO o null, sin importar si el producto esta habilitado o no*/

            //Creamos el producto resultante
            ProductoDTO miProducto;

            //Instanciamos la base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos el producto en la base
                var ProductoBase = miBase.Producto.Where(prod => prod.id == idProducto).FirstOrDefault();

                //Si no encontramos el producto retornamos null:
                if (ProductoBase == null)
                {
                    miProducto = null;
                }
                //Si encontramos el producto hacemos el mapping correspondiente para pasarlo a ProductoDTO:
                else
                {
                    miProducto = new ProductoDTO()
                    {
                        Id = ProductoBase.id,
                        Marca = ProductoBase.marca,
                        Nombre = ProductoBase.nombre,
                        Precio = ProductoBase.precio,
                        NickUsuario = ProductoBase.nickUsuario,
                        Habilitado = ProductoBase.habilitado
                    };

                }

                //Devolvemos el producto
                return miProducto;
            }
        }

        //Crear Producto
        public void CrearProducto(ProductoDTO nuevoProducto)
        {
            //Instanciamos la base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Creamos un Producto
                Producto unProducto = new Producto()
                {
                    id = nuevoProducto.Id,
                    marca = nuevoProducto.Marca,
                    nombre = nuevoProducto.Nombre,
                    precio = nuevoProducto.Precio,
                    nickUsuario = nuevoProducto.NickUsuario,
                    /*Esta habilitado porque lo estamos creando*/
                    habilitado = true
                };

                //Agregamos el producto a la base
                miBase.Producto.Add(unProducto);

                //Guardamos los cambios
                miBase.SaveChanges();
            }
        }

        public List<ProductoDTO> ListaProductos()
        {
            /*Devuelve la lista de productos esten registrados o no*/

            //Instanciamos la base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Traemos desde la base de datos la lista de productos convertida a List<ProductoDTO>
                var lista = from unProducto in miBase.Producto
                            select new ProductoDTO()
                            {
                                Id = unProducto.id,
                                Nombre = unProducto.nombre,
                                Marca = unProducto.marca,
                                NickUsuario = unProducto.nickUsuario,
                                Precio = unProducto.precio,
                                Habilitado = unProducto.habilitado
                            };

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

        public void EditarProducto(ProductoDTO unProducto)
        {
            /*Solo editamos productos habilitados*/

            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos el producto en nuestra base
                var productoEditar = miBase.Producto.Where(prod => prod.id == unProducto.Id).FirstOrDefault();

                //Lo fijamos (Nos paramos en el)
                miBase.Producto.Attach(productoEditar);

                //Re-Mapeamos su contenido con los datos nuevos
                productoEditar.id = unProducto.Id;
                productoEditar.marca = unProducto.Marca;
                productoEditar.nombre = unProducto.Nombre;
                productoEditar.nickUsuario = unProducto.NickUsuario;
                productoEditar.precio = unProducto.Precio;
                productoEditar.habilitado = true; //Porque solo editamos productos habilitados

                //Salvamos los cambios
                miBase.SaveChanges();
            }
        }

        public void EliminarProducto(int id)
        {
            /*Toma el producto con esa id y cambia el valor de habilitado a false*/

            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos y seleccionamos el producto a eliminar
                var prodBorrar = (from unProd in miBase.Producto where unProd.id == id select unProd).First();

                //Fijamos el producto
                miBase.Producto.Attach(prodBorrar);

                //Cambiamos el valor de habilitado a false
                prodBorrar.habilitado = false;

                //Guardamos los cambios
                miBase.SaveChanges();
            }
        }

        public void HabilitarProducto(int Producto)
        {
            /*Restaura (vuelve a habilitar) un producto deshabilitado*/

            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos y seleccionamos el producto a habilitar
                var prodBorrar = (from unProd in miBase.Producto where unProd.id == Producto select unProd).First();

                //Fijamos el producto
                miBase.Producto.Attach(prodBorrar);

                //Cambiamos el valor de habilitado a true
                prodBorrar.habilitado = true;

                //Guardamos los cambios
                miBase.SaveChanges();
            }
        }

        //Devolveremos la lista de productos que registro un cliente x que nos pasan por parametro
        public List<ProductoDTO> ProductosUsuario(string usuario)
        {
            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Creamos la lista de todos los productos que hayan sido registrados por el usuario
                var lista = from unProducto in miBase.Producto
                            where unProducto.nickUsuario.Trim().ToLower() == usuario.Trim().ToLower()
                            select new ProductoDTO()
                            {
                                Habilitado = unProducto.habilitado,
                                Id = unProducto.id,
                                Marca = unProducto.marca,
                                NickUsuario = unProducto.nickUsuario,
                                Nombre = unProducto.nombre,
                                Precio = unProducto.precio
                            };
                //Devolvemos la lista que acabamos de crear
                return lista.ToList();
            }
        }

        /*Datos*/
        public int ProductosRegistrados()
        {
            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Consultamos la cantidad de productos registrados
                var res = miBase.Producto.Count();

                //Devolvemos esa cantidad
                return res;
            }
        }

    }
}
