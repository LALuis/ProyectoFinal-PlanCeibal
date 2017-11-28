using CD.Repositorio;
using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Logicas
{
    public class LogicaProducto
    {
        //Creamos una instancia del Repositorio de Productos
        RepoProducto miRepo = new RepoProducto();

        /*Buscamos y retornamos un producto segun su id*/
        public ProductoDTO BuscarProducto(int idProducto)
        {
            return miRepo.BuscarProducto(idProducto);
        }

        /*Registramos el producto que nos pasan como parametro*/
        public void CrearProducto(ProductoDTO nuevoProducto)
        {
            miRepo.CrearProducto(nuevoProducto);
        }

        /*Devolvemos la lista de TODOS los productos registrados en el sistema*/
        public List<ProductoDTO> ListaProductos()
        {
            return miRepo.ListaProductos();
        }

        /*Editamos los valores de un producto, reescribiendolo con los datos del producto que nos pasan como parametro*/
        public void EditarProducto(ProductoDTO unProducto)
        {
            miRepo.EditarProducto(unProducto);
        }

        /*Deshabilitamos  el producto cuyo id nos pasan como parametro*/
        public void EliminarProducto(int id)
        {
            miRepo.EliminarProducto(id);
        }

        /*Habilitamos el producto cuyo id nos pasan como parametro*/
        public void HabilitarProducto(int Producto)
        {
            miRepo.HabilitarProducto(Producto);
        }

        //Lista de productos registrados por un usuario en concreto
        public List<ProductoDTO> ProductosUsuario(string usuario)
        {
            return miRepo.ProductosUsuario(usuario);
        }

        /*DATOS*/
        public int ProductosRegistrados()
        {
            return miRepo.ProductosRegistrados();
        }
    }
}
