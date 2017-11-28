using CL.Logicas;
using EC;
using FacturacionBOOSTRAP.HerramientasModel;
using FacturacionBOOSTRAP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacturacionBOOSTRAP.Controllers
{
    public class ProductoController : Controller
    {
        //Instanciamos la logica
        LogicaProducto miLogica = new LogicaProducto();

        // GET: CrearProducto
        public ActionResult CrearProducto()
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Retornamos la vista
                return View();
            }
            else //Si el usuario no esta logueado
            {
                //Redirigimos al login
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //POST: CrearProducto
        [HttpPost]
        public ActionResult CrearProducto(ProductoModel unProducto)
        {
            //YA controlamos que el usuario este logueado en el GET

            //Si el modelo no es valido retornamos la vista con los errores
            if (!ModelState.IsValid)
            {
                //Retornamos el modelo con sus errores
                return View(unProducto);
            }
            else //Si el modelo ES VALIDO
            {
                //Nos anticipamos a un posible error en DB
                try
                {
                    //Buscamos el producto por su ID, si hay alguno con la misma id mostramos error
                    ProductoDTO buscando = miLogica.BuscarProducto(unProducto.Id);
                    if (buscando == null)
                    {
                        //Creamos un ProductoDTO para pasarlo a la logica
                        ProductoDTO nuevoProducto = new ProductoDTO()
                        {
                            Id = unProducto.Id,
                            Marca = unProducto.Marca,
                            //Ya controlamos que la session no sea nula entonces:
                            NickUsuario = HerramientasM.IdLogueado((UsuarioModel)Session["Logueado"]),
                            Nombre = unProducto.Nombre,
                            Precio = unProducto.Precio,
                            Habilitado = true
                        };

                        //Enviamos el producto a la logica
                        miLogica.CrearProducto(nuevoProducto);

                        //Redirigimos al Action para que se reseteen los campos del formulario
                        return RedirectToAction("CrearProducto");
                    }
                    else //Si el producto con esa id esta en el sistema
                    {
                        //Creamos el mensaje de error
                        ModelState.AddModelError("Id", $"Ya existe un producto con id: {unProducto.Id}");
                        //Devolvemos la vista
                        return View(unProducto);
                    }
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en base de datos
                    //Retornamos la vista de error
                    return View("Error");
                }
            }
        }

        //GET: EditarProducto
        public ActionResult EditarProducto(int Producto)
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Nos anticipamos a un posible error de nuestra base de datos
                try
                {
                    //Buscamos nuestro producto en la base, 
                    //lo convertimos a ProductoModel con nuestro metodo en HerramientasM y lo pasamos a la vista
                    return View(HerramientasM.ConvertirProductoDTO(miLogica.BuscarProducto(Producto)));
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
            else //Si el usuario no se logueo
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //POST: EditarProducto
        [HttpPost]
        public ActionResult EditarProducto(ProductoModel unProducto)
        {
            //Verificamos que el modelo que nos llega sea valido
            if (!ModelState.IsValid)
            {
                //En caso de que no sea valido retornamos la vista con el modelo
                return View(unProducto);
            }
            else //Si el modelo es valido
            {
                //Anticipamos un posible error en nuestra base
                try
                {
                    //Convertimos nuestro ProductoModel a ProductoDTO y
                    //Le pasamos nuestro producto a la logica
                    miLogica.EditarProducto(HerramientasM.ConvertirProductoModel(unProducto));

                    //retornamos la vista con todos los productos
                    return RedirectToAction("MostrarProductos");
                }
                catch(Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Retornamos la vista de error
                    return View("Error");
                }
            }
        }

        //GET: EliminarProducto (Mostraremos el detalle del producto que queremos eliminar)
        public ActionResult EliminarProducto(int Producto)
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Anticipamos un posible error en servidor
                try
                {
                    //Buscamos el Producto en la base de datos
                    ProductoDTO unProducto = miLogica.BuscarProducto(Producto);

                    //Convertimos el ProductoDTO a ProductoModel y lo pasamos a la vista
                    return View(HerramientasM.ConvertirProductoDTO(unProducto));
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base de datos
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
            else //Si el usuario no se logueo
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //Confirmar Eliminar Producto
        public ActionResult ConfirmaEliminarProducto(int Producto)
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                    //Anticipamos un posible error en el servidor
                    try
                    {
                        //Le pasamos a la logica la id del producto para que lo borre
                        miLogica.EliminarProducto(Producto);

                        //Retornamos la vista con la lista de productos
                        return RedirectToAction("MostrarProductos");
                    }
                    catch (Exception unError)
                    {
                        //Podriamos guardar el error en la base
                        //Redirigimos a vista de error
                        return View("Error");
                    }
            }
            else //Si el usuario no se logueo o no es admin
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        public ActionResult MostrarProductos()
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Anticipamos a un posible error en la base de datos
                try
                {
                    //Traemos la lista de productos desde nuestra logica
                    List<ProductoDTO> listaProductoDTO = miLogica.ListaProductos();

                    //Convertimos la lista que nos llego en una lista de productos habilitados
                    List<ProductoModel> lista = HerramientasM.ProductosHabilitados(listaProductoDTO);

                    //Pasamos la lista a la vista
                    return View(lista);
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Redirigimos a la vista de error
                    return View("Error");
                }

            }
            else //Si no esta logueado
            {
                //Redirigimos al login
                return RedirectToAction("Login", "Home");
            }
        }

        /*Mostraremos la lista de productos deshabilitados*/
        public ActionResult MostrarProductosDeshabilitados()
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Anticipamos a un posible error en la base de datos
                try
                {
                    //Traemos la lista de productos desde nuestra logica
                    List<ProductoDTO> listaProductoDTO = miLogica.ListaProductos();

                    //Convertimos la lista que nos llego en una lista de productos deshabilitados
                    List<ProductoModel> lista = HerramientasM.ProductosDeshabilitados(listaProductoDTO);

                    //Pasamos la lista a la vista
                    return View(lista);
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Redirigimos a la vista de error
                    return View("Error");
                }

            }
            else //Si no esta logueado
            {
                //Redirigimos al login
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult HabilitarProducto(int Producto)
        {
            //Verificamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Prevenimos un posible error en nuestra base de datos
                try
                {
                    //Pasamos el id de cliente a la logica
                    miLogica.HabilitarProducto(Producto);
                    //Retornamos la vista con el listado de clientes deshabilitados
                    return RedirectToAction("MostrarProductosDeshabilitados");
                }
                catch (Exception unError)//podriamos guardar la excepcion en nuestra base de datos
                {
                    //redirigimos a la vista de error
                    return View("Error");
                }
            }
            else
            {
                //Vamos a la ventana de login
                return RedirectToAction("Login", "Ingreso");
            }

        }

        /* En esta seccion se definiran los PartialViewResult que nos seran de gran ayuda*/

        /*Mostraremos una PartialView con la lista de productos habilitados*/
        public PartialViewResult ListadoProductos()
        {
            /*Devolveremos una vista parcial que contendra la tabla con los clientes registrados*/
            //No precisamos verificar el login porque es una partial view

            //Prevenimos un posible error en nuestra base de datos
            try
            {
                //Pedimos la lista de productos en nuestra base
                List<ProductoDTO> miLista = miLogica.ListaProductos();

                //Creamos una lista solo con los productos registrados habilitados
                List<ProductoModel> lista = HerramientasM.ProductosHabilitados(miLista);

                //Le pasamos la lista a la vista
                return PartialView("_ListadoProductos", lista);
            }
            catch (Exception unError)//podriamos guardar la excepcion en nuestra base de datos
            {
                //redirigimos a la vista PARCIAL de error
                return PartialView("_Error");
            }
        }

        /*Mostraremos los productos registrados por el usuario que nos llega como parametro*/
        public PartialViewResult HistorialUsuarioProducto(string Usuario)
        {
            //Pedimos la lista de Productos para este usuario en nuestra base
            //Anticipamos un posible error en nuestra base de datos
            try
            {
                //Pedimos la lista de productos habilitados para este usuario y la convertimos a producto model(porque viene como productosDTO)
                List<ProductoModel> lista = HerramientasM.ProductosHabilitados(miLogica.ProductosUsuario(Usuario));

                //Retornamos la vista parcial de productos
                return PartialView("_ListadoProductos",lista);
            }
            catch(Exception unError)
            {
                //podriamos guardar el error en la base de datos
                //Retornamos la vista parcial de error
                return PartialView("_Error");
            }
        }
    }
}