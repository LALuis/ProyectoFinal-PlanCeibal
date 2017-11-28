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
    public class ClienteController : Controller
    {
        //Instanciamos nuestra Logica
        LogicaCliente miLogica = new LogicaCliente();

        // GET: CrearCliente
        public ActionResult CrearCliente()
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Retornamos la vista
                return View();
            }
            else //Si el usuario no se logueo
            {
                //Redirigimos al login
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //POST: CrearCliente
        [HttpPost]
        public ActionResult CrearCliente(ClienteModel unCliente)
        {
            //Si el modelo NO es valido
            if (!ModelState.IsValid)
            {
                //Retornamos la vista con error
                return View(unCliente);
            }
            else //Si el modelo esta bien
            {
                //Nos anticipamos a un posible error en nuestra base
                try
                {
                    //Buscamos para ver si el cliente ya esta registrado
                    ClienteDTO buscar = miLogica.BuscarCliente(unCliente.Cedula);

                    //Si el cliente YA ESTA registrado devolvemos el error
                    if (buscar != null)
                    {
                        //Creamos el mensaje de error para el campo Cedula
                        ModelState.AddModelError("Cedula", $"Ya existe un cliente con cedula {unCliente.Cedula}");

                        //Devolvemos la vista con el error
                        return View(unCliente);
                    }
                    else //Si el cliente NO ESTA registrado
                    {
                        //Verificamos que el cliente sea mayor de edad
                        if (FuncionesAuxiliares.MayorDeEdad(unCliente.FechaNacimiento))
                        {
                            //Pasamos de ClienteModel a ClienteDTO para enviarlo a la logica
                            ClienteDTO nuevoCliente = new ClienteDTO()
                            {
                                Cedula = unCliente.Cedula,
                                Nombre = unCliente.Nombre,
                                Domicilio = unCliente.Domicilio,
                                FechaNacimiento = unCliente.FechaNacimiento,
                                //Llamamos a la funcion que nos devuelve el nombre del usuario logueado
                                NickUsuario = HerramientasM.IdLogueado((UsuarioModel)Session["Logueado"]),
                                Habilitado = true
                            };

                            //Pasamos el nuevo cliente a la logica
                            miLogica.CrearCliente(nuevoCliente);

                            //Redirigimos al GET para que se reseteen los campos
                            return RedirectToAction("CrearCliente");
                        }
                        else //Si no es mayor de edad 
                        {
                            //Creamos el mensaje de error para el campo Fecha de Nacimiento
                            ModelState.AddModelError("FechaNacimiento", $"El cliente debe ser mayor de edad");

                            //Devolvemos la vista con el error
                            return View(unCliente);
                        }
                    }
                }
                catch (Exception unError)
                {
                    //Podriamos guardar nuestro error en la base de datos
                    //Retornamos la vista de error
                    return View("Error");
                }
            }
        }

        //GET: EditarCliente
        public ActionResult EditarCliente(int Cliente)
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Nos anticipamos a un posible error en nuestra base de datos
                try
                {
                    //Buscamos nuestro cliente en la base
                    //Convertimos nuestro ClienteDTO a ClienteModel con nustro metodo en HerramientasM y lo pasamos a la vista
                    return View(HerramientasM.ConvertirClienteDTO(miLogica.BuscarCliente(Cliente)));
                }
                catch (Exception unError)
                {
                    //Podriamos guradar el error en nuestra base de datos
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
            else //Si el usuario no se logueo
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //POST: EditarCliente
        [HttpPost]
        public ActionResult EditarCliente(ClienteModel unCliente)
        {
            //Verificamos que el cliente sea correcto
            if (!ModelState.IsValid)
            {
                //Si el modelo no es correcto retornamos la vista y le pasamos el modelo con el error
                return View(unCliente);
            }
            else //Si el modelo es correcto
            {
                //Anticipamos un posible error de la base de datos
                try
                {
                    //Convertimos nuestro ClienteModel a ClienteDTO con HerramientasM y lo pasamos a la logica
                    miLogica.EditarCliente(HerramientasM.ConvertirClienteModel(unCliente));

                    //Retornamos la vista MostrarClientes
                    return RedirectToAction("MostrarClientes");
                }
                catch (Exception unError)
                {
                    //Podriamos guradar el error en nuestra base de datos
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
        }

        //GET: EliminarCliente
        public ActionResult EliminarCliente(int Cliente)
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Anticipamos un posible error en la base de datos
                try
                {
                    //Buscamos el cliente en la base de datos, lo convertimos a clienteMODEL y se lo pasamos a la vista 
                    return View(HerramientasM.ConvertirClienteDTO(miLogica.BuscarCliente(Cliente)));
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

        //Confirmar EliminarCliente
        public ActionResult ConfirmarEliminarCliente(int Cliente)
        {
            //Controlamos que el modelo sea Valido
            if (ModelState.IsValid)
            {
                //Prevenimos un posible error en nuestra base
                try
                {
                    //Pasamos el id del cliente a nuestra logica
                    miLogica.EliminarCliente(Cliente);

                    //Redirigimos a la vista con la lista de clientes
                    return RedirectToAction("MostrarClientes");
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en nuestra base de datos
                    //Retornamos la vista de error
                    return View("Error");
                }
            }
            else //Si el modelo no es valido
            {
                //Retornamos la vista pasando el modelo que lleva el error
                return View(Cliente);
            }
        }

        //GET: mostrar cliente muestra los clientes habilitados
        public ActionResult MostrarClientes()
        {
            //Verificamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Nos anticipamos a un posible error en nuestra base de datos
                try
                {
                    //Pedimos la lista de clientes en nuestra base
                    List<ClienteDTO> miLista = miLogica.ListaClientes();

                    //Creamos una lista solo con los usuarios habilitados y convertia a List<ClienteModel>
                    List<ClienteModel> lista = HerramientasM.ClientesHabilitados(miLista);

                    //Le pasamos la lista a la vista
                    return View(lista);
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en nuestra base de datos
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
            else//Si el usuario no esta logueado
            {
                //Redirigimos al login
                return RedirectToAction("Login", "Ingreso");
            }
        }

        /*Muestra los clientes que no estan habilitados*/
        public ActionResult MostrarClientesDeshabilitados()
        {
            //Verificamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Anticipamos un posible error en nuestra base de datos
                try
                {
                    /*Llamamos a nuestro metodo ClientesDeshabilitados de HerramientasM
                    * que nos devuelve una lista ClienteModel de clientes deshabilitados*/
                    List<ClienteModel> miLista = HerramientasM.ClientesDeshabilitados(miLogica.ListaClientes());

                    //Retornamos la vista con la lista correspondiente de clientes deshabilitados
                    return View(miLista);
                }
                catch (Exception unError) //Podriamos guardar el error en la base de datos
                {
                    //Devolvemos la vista de error
                    return View("Error");
                }
            }
            else//Si el usuario no esta logueado
            {
                //Redirigimos al login
                return RedirectToAction("Login", "Ingreso");
            }
        }

        /*Habilita el cliente cuya id nos llega como parametro*/
        public ActionResult HabilitarCliente(int Cliente)
        {
            //Verificamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Prevenimos un posible error en nuestra base de datos
                try
                {
                    //Pasamos el id de cliente a la logica
                    miLogica.HabilitarCliente(Cliente);

                    //Retornamos la vista con el listado de clientes deshabilitados
                    return RedirectToAction("MostrarClientesDeshabilitados");
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

        /*Muestra la lista de clientes habilitados*/
        public PartialViewResult ListadoClientes()
        {
            /*Devolveremos una vista parcial que contendra la tabla con los clientes registrados habilitados*/
            //No precisamos verificar el login porque es una partial view

            //Prevenimos un posible error en nuestra base de datos
            try
            {
                //Pedimos la lista de clientes en nuestra base
                List<ClienteDTO> miLista = miLogica.ListaClientes();

                //Creamos una lista solo con los clientes registrados
                List<ClienteModel> lista = HerramientasM.ClientesHabilitados(miLista);

                //Le pasamos la lista a la vista
                return PartialView("_ListadoClientes", lista);
            }
            catch (Exception unError)//podriamos guardar la excepcion en nuestra base de datos
            {
                //redirigimos a la vista PARCIAL de error
                return PartialView("_Error");
            }
        }

        /*Nos devolvera la lista de clientes habilitados creados por un usuario en especifico*/
        public PartialViewResult HistorialUsuarioCliente(string Usuario)
        {
            //Pedimos la lista de Clientes para este usuario en nuestra base
            //Anticipamos un posible error en nuestra base de datos
            try
            {
                //Pedimos la lista de Clientes Habilitados para este usuario y la convertimos a ClienteModel(porque viene como ClienteDTO)
                List<ClienteModel> lista = HerramientasM.ClientesHabilitados(miLogica.ClientesUsuario(Usuario));

                //Retornamos la vista parcial de clientes
                return PartialView("_ListadoClientes", lista);
            }
            catch (Exception unError)
            {
                //podriamos guardar el error en la base de datos
                //Retornamos la vista parcial de error
                return PartialView("_Error");
            }
        }
    }
}