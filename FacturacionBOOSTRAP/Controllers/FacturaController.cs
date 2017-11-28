using CL.Herramientas;
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
    public class FacturaController : Controller
    {
        /*Instanciamos la logica de Facturas*/
        LogicaFactura logicaFactura = new LogicaFactura();

        //GET: CrearFactura, crearemos la factura
        public ActionResult CrearFactura()
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Creamos la factura vacia
                FacturaModel nuevaFactura = new FacturaModel();
                //inicializamos la lista de productos
                nuevaFactura.ListaDetalle = new List<DetalleFacturaModel>();

                //Cargamos la lista de clientes
                //Como usaremos la base de datos prevenimos un posible error
                try
                {
                    //Cargamos la lista de clientes para el dropdown - ListaClientes() es una metodo que esta en este controlador
                    nuevaFactura.ListaClientes = ListaClientes();

                    //Cargamos la lista de productos para el dropdown - ListaProductos() es una metodo que esta en este controlador
                    nuevaFactura.ListaProductos = ListaProductos();

                    //Creo una session donde guardare temporalmente mi factura
                    Session.Add("nuevaFactura", nuevaFactura);

                    //Pasamos la factura a la vista para que cargue los selectlist
                    return View(nuevaFactura);
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Retornamos la vista de error
                    return View("Error");
                }
            }
            else
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //POST: CrearFactura, agregaremos el producto a la factura
        [HttpPost]
        public ActionResult CrearFactura(FacturaModel nuevaFactura)
        {
            //Controlamos que el usuario este logueado, en este caso no importa si es admin o no
            if (Session["Logueado"] != null)
            {
                if (ModelState.IsValid)
                {
                    //Nos anticipamos a cualquier error inesperado que pueda ocurrir en base usando try
                    try
                    {
                        //Creamos una instancia de la logica de productos
                        LogicaProducto logProductos = new LogicaProducto();

                        //Busco el producto ingresado en mi base de datos
                        ProductoDTO productoActual = logProductos.BuscarProducto(nuevaFactura.IdProducto);
                        //Sabemos que el producto no viene null porque el id es seleccionado de una lista. NO es ingresado por el usuario

                        //Creamos el detalle y le cargamos los datos
                        DetalleFacturaModel nuevoDetalle = new DetalleFacturaModel()
                        {
                            IdProducto = productoActual.Id,
                            Cantidad = nuevaFactura.Cantidad,
                            SubTotal = productoActual.Precio * nuevaFactura.Cantidad
                        };

                        //Creamos una factura para asignarle lo que esta en session
                        FacturaModel unaFactura = new FacturaModel();
                        unaFactura = (FacturaModel)Session["nuevaFactura"];

                        //Vemos si la lista de productos de la factura esta creada, sino la creamos y agregamos nuestro detalle(Porque si es null y tratamos de agregarle algo va a explotar)
                        if (unaFactura.ListaDetalle == null)
                        {
                            //creamos la lista
                            unaFactura.ListaDetalle = new List<DetalleFacturaModel>();
                            //Se lo pasamos a nuestra factura
                            unaFactura.ListaDetalle.Add(nuevoDetalle);
                        }
                        else
                        {
                            //Si ya tiene algun item agregamos normalmente
                            unaFactura.ListaDetalle.Add(nuevoDetalle);
                        }

                        //Actualizamos el gastototal, La funcion calcularGasto no esta en la capa logica porque usaremos un DetalleFacturaModel y la logica no tiene acceso...
                        unaFactura.Total = CalcularGasto(unaFactura.ListaDetalle);

                        //Guardamos los datos en la session
                        Session["nuevaFactura"] = unaFactura;

                        //Retornamos la vista:
                        return View(unaFactura);
                    }
                    catch
                    {
                        //Redirigimos a la vista de error...
                        return View("Error");
                    }

                }
                else //Si el modelo no es valido
                {
                    //Devolvemos la vista con los errores correspondientes 
                    return View(nuevaFactura);
                }
            }
            else//Si no esta logueado
            {
                //Redirigimos a la vista de LOGIN para que ingrese
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //POST: GuardarFactura(guarda la factura que creamos)
        [HttpPost]
        public ActionResult GuardarFactura(FacturaModel unaFactura)
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                if (ModelState.IsValid)
                {
                    //Nos anticipamos a un posible error en servidor
                    try
                    {
                        //Guardamos el usuario actual para agregarlo a la factura
                        UsuarioModel usuarioActual = (UsuarioModel)Session["Logueado"];

                        //Creamos una facturaDTO la cual pasaremos a la base para que se guarde
                        FacturaDTO miFactura = new FacturaDTO()
                        {
                            Fecha = DateTime.Now,
                            NickUsuario = usuarioActual.Usuario,
                            NumeroCliente = unaFactura.NumeroCliente,
                            Total = CalcularGasto(unaFactura.ListaDetalle)
                        };

                        //Convertimos la lista del tipo List<DetalleFacturaModel> al tipo List<DetalleFacturaDTO> creando una nueva lista y agregandola a nuestra factura
                        //Inicializamos la lista:
                        miFactura.ListaDetalle = new List<DetalleFacturaDTO>();

                        //Vamos agregando uno a uno los detalles
                        foreach (DetalleFacturaModel item in unaFactura.ListaDetalle)
                        {
                            //Creamos el DetalleFacturaDTO
                            DetalleFacturaDTO nuevoDetalle = new DetalleFacturaDTO()
                            {
                                IdProducto = item.IdProducto,
                                Cantidad = item.Cantidad,
                                SubTotal = item.SubTotal
                                //Omitimos id de factura porque se agrega en Capa de Datos
                            };
                            //Agregamos el detalle a la lista
                            miFactura.ListaDetalle.Add(nuevoDetalle);
                        }

                        //Ahora que ya tenemos la factura lista la enviamos a guardar en la base de datos
                        logicaFactura.AgregarFactura(miFactura);

                        //Si todo salio bien redirigimos a crear factura para que pueda agregar mas
                        return RedirectToAction("CrearFactura", "Factura");
                    }
                    catch (Exception unError)
                    {
                        //Podemos guardar el error en la base de datos
                        //Redirigimos a la vista de error
                        return View("Error");
                    }
                }
                else//Si el modelo no es valido
                {
                    //Redirigimos a crear factura
                    return RedirectToAction("CrearFactura", "Factura");
                }
            }
            else//Si el usuario no esta logueado
            {
                //Si el usuario no esta logueado redirigimos a el login
                return RedirectToAction("Login", "Ingreso");
            }

        }

        public ActionResult MostrarFactura(string numeroFactura)
        {
            //Anticipamos un posible error en la base de datos
            try
            {
                //Buscamos la factura en la base:
                FacturaDTO miFactura = logicaFactura.BuscarFactura(Convert.ToInt32(numeroFactura));

                //Convertimos la factura a FacturaModel y ademas le agregamos el nombre de cliente y la descripcion del producto
                FacturaModel unaFacturaModel = HerramientasM.ConvertirAFacturaModel(miFactura);

                //Retornamos la vista parcial:
                return View("_MostrarFactura", unaFacturaModel);
            }
            catch (Exception unError)
            {
                //Podriamos guardar el error
                //Devolvemos la vista de error
                return PartialView("_Error");
            }
        }

        /*Comienzan los PARTIALVIEWRESULT*/

        public PartialViewResult ListadoFacturas()
        {
            try
            {
                //Pedimos la lista a la logica
                List<FacturaDTO> unaLista = logicaFactura.ListaFacturas();

                //Convertimos esa lista a Model
                List<FacturaModel> lista = HerramientasM.ListaFacturaModel(unaLista);

                //Retornamos una vista pasando como parametro la lista de facturas
                return PartialView("_ListadoFacturas",lista);
            }
            catch(Exception unError)
            {
                return PartialView("_Error");
            }
        }

        /*Metodos auxiliares*/

        private decimal CalcularGasto(List<DetalleFacturaModel> unaLista)
        {
            //Para cada item de la lista, le sumamos su subtotal a resultado.

            Decimal resultado = 0;

            foreach (DetalleFacturaModel item in unaLista)
            {
                resultado += item.SubTotal;
            };

            return resultado;

        }

        private List<SelectListItem> ListaClientes()
        {
            //Creamos la lista resultante
            List<SelectListItem> resultado = new List<SelectListItem>();

            //Instanciamos la logica de clientes
            LogicaCliente unaLogica = new LogicaCliente();

            //Traemos la lista de clientes
            List<ClienteDTO> listaDTO = unaLogica.ListaClientes();

            //Filtramos solo los clientes habilitados
            List<ClienteModel> lista = HerramientasM.ClientesHabilitados(listaDTO);

            foreach (ClienteModel unCliente in lista)
            {
                //Agregamos los clientes habilitados a la lista
                resultado.Add(new SelectListItem
                {
                    Text = (unCliente.Cedula + " - " + unCliente.Nombre),
                    Value = unCliente.Cedula.ToString(),
                    Selected = false
                });
            }

            //Retornamos la lista
            return resultado;
        }

        private List<SelectListItem> ListaProductos()
        {
            //Creamos la lista resultante
            List<SelectListItem> resultado = new List<SelectListItem>();

            //Instanciamos la logica de productos
            LogicaProducto unaLogica = new LogicaProducto();

            //Traemos la lista de productos
            List<ProductoDTO> listaDTO = unaLogica.ListaProductos();

            //Filtramos solo los productos habilitados
            List<ProductoModel> lista = HerramientasM.ProductosHabilitados(listaDTO);

            foreach (ProductoModel unProducto in lista)
            {
                //Agregamos los productos habilitados a la lista
                resultado.Add(new SelectListItem
                {
                    Text = (unProducto.Id + " - " + unProducto.Nombre + " " + unProducto.Marca),
                    Value = unProducto.Id.ToString(),
                    Selected = false
                });
            }

            //Retornamos la lista
            return resultado;
        }

        public PartialViewResult HistorialUsuarioFactura(string Usuario)
        {
            //Pedimos la lista de Facturas para este usuario en nuestra base
            //Anticipamos un posible error en nuestra base de datos
            try
            {
                //Pedimos la lista de Facturas para este usuario y la convertimos a Factura model(porque viene como FacturaDTO)
                List<FacturaModel> lista = HerramientasM.ListaFacturaModel(logicaFactura.FacturasUsuario(Usuario));

                //Retornamos la vista parcial de las facturas
                return PartialView("_ListadoFacturas", lista);
            }
            catch (Exception unError)
            {
                //podriamos guardar el error en la base de datos
                //Retornamos la vista parcial de error
                return PartialView("_Error");
            }
        }

        /*IMPRIMIR FACTURA*/

        public ActionResult Pdf(int numeroFactura)
        {
            //Buscamos la factura en la base:
            FacturaDTO miFactura = logicaFactura.BuscarFactura(Convert.ToInt32(numeroFactura));

            //Retornamos el pdf de la vista ImprimirFactura con los datos de mifactura
            return new Rotativa.ActionAsPdf("ImprimirFactura",miFactura);
        }

        public ActionResult ImprimirFactura(FacturaDTO miFactura)
        {
            //Buscamos la factura en la base:
            FacturaDTO unaFactura = logicaFactura.BuscarFactura(miFactura.numeroFactura);

            //Convertimos la FacturaDTO a Facturamodel con el metodo en HerramientasM
            //Este metodo tambien agregara la descripcion del producto y el nombre del cliente (info importante a la hora de listar)
            FacturaModel unaFacturaModel = HerramientasM.ConvertirAFacturaModel(unaFactura);

            //Retornamos la vista parcial:
            return View("_ImprimirFactura", unaFacturaModel);
        }

        /*Nos devuelve un string con Nombre - Marca de un producto correspondiente al id que nos llega como parametro*/
        private string NombreMarcaProducto(int idProducto)
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
            catch(Exception)
            {
                //Retornamos un error para saber que algo fallo
                return ("Algo salio mal");
            }
        }

        /*Nos retorna el nombre del cliente correspodiente al id que nos llega como parametro*/
        private string NombreCliente(int idCliente)
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
    }
}