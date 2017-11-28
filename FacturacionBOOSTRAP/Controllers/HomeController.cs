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
    public class HomeController : Controller
    {
        //Pagina de inicio
        public ActionResult Index()
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

        //Menu donde podemos elegir crear factura, producto, usuario o cliente
        public ActionResult MenuNuevo()
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Usamos un try porque intentaremos desde el constructor tomar datos de la base y puede haber una falla
                try
                {
                    //Instanciamos el modelo de datos
                    DatosModel misDatos = new DatosModel();

                    //Le pasamos los datos a la vista para que los muestre
                    return View(misDatos);
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    return View("Error");
                }
            }
            else //Si el usuario no se logueo
            {
                //Redirigimos al login
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //Menu donde podemos editar facturas, productos, usuarios o clientes
        public ActionResult MenuEditar()
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                return View();
            }
            else //Si el usuario no se logueo
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        /*Menu donde tendremos todas las listas del sistema*/
        public ActionResult Buscar()
        {
            //Controlamos que el usuario este logueado
            if (Session["Logueado"] != null)
            {
                //Retornamos la vista
                return View();
            }
            else //Si el usuario no se logueo
            {
                //Vamos a la vista de Login
                return RedirectToAction("Login", "Ingreso");
            }
        }

        /*Menu donde podremos elegir un usuario y ver sus registros*/
        public ActionResult Historial()
        {
            //Controlamos que el usuario este logueado y sea admin
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Vamos a manejar los datos asi que nos anticipamos a un posible error
                try
                {
                    //Tenemos que crear el selectlistitem agregarlo al modelo y pasarlo a la vista para que se pueda seleccionar el usuario que queremos ver
                    //Cargamos la lista de Usuarios para el dropdown - ListaUsuarios() es una metodo que esta en este controlador
                    //Creamos un usuario Generico para cargarle la lista
                    UsuarioModel unUsuario = new UsuarioModel()
                    {
                        ListaUsuarios = ListaUsuarios()
                    };

                    //Pasamos el usuario a la vista
                    return View(unUsuario);
                }
                catch(Exception unError)
                {
                    //Podriamos guardar el error en la base de datos
                    //Retornamos la vista de error
                    return View("Error");
                }
                
            }
            else //Si el usuario no se logueo
            {
                //Lo enviamos a la vista de login
                return RedirectToAction("Login", "Ingreso");
            }
        }

        /*Luego de que elegimos el usuario se hace un post y aqui pasamos el usuario que nos llega a la misma vista como string
         * para que muestre(a travez de partial views) sus registros*/
        [HttpPost]
        public ActionResult Historial(string Usuario)
        {
            //Vamos a manejar los datos asi que nos anticipamos a un posible error
            try
            {
                //Tenemos que crear el selectlistitem agregarlo al modelo y pasarlo a la vista para que se pueda seleccionar el usuario que queremos ver
                //Cargamos la lista de Usuarios para el dropdown - ListaUsuarios() es una metodo que esta en ESTE controlador
                //Creamos un usuario Generico para cargarle la lista
                UsuarioModel unUsuario = new UsuarioModel()
                {
                    Usuario = Usuario,
                    ListaUsuarios = ListaUsuarios()
                };

                //Pasamos el usuario a la vista
                return View(unUsuario);
            }
            catch (Exception unError)
            {
                //Podriamos guardar el error en la base de datos
                //Retornamos la vista de error
                return View("Error");
            }
        }

        /*Menu datos, aqui mostraremos algunos datos del negocio*/
        public ActionResult Datos()
        {
            //Controlamos que el usuario este logueado y sea admin
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Usamos un try porque intentaremos desde el constructor tomar datos de la base y puede haber una falla
                try
                {
                    //Instanciamos el modelo de datos y el propio constructor completara esos datos consultando la base
                    DatosModel misDatos = new DatosModel();

                    //Le pasamos los datos a la vista para que los muestre
                    return View(misDatos);
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    return View("Error");
                }
            }
            else
            {
                //Redirigimos a la vista de index
                return RedirectToAction("Index");
            }
        }

        /*Metodo auxiliar que nos crea una lista de selectlistitem con los usuarios habiitados del sistema*/
        private List<SelectListItem> ListaUsuarios()
        {
            //Creamos la lista resultante
            List<SelectListItem> resultado = new List<SelectListItem>();

            //Instanciamos la logica de Usuarios
            LogicaUsuario unaLogica = new LogicaUsuario();

            //Traemos la lista de Usuarios
            List<UsuarioDTO> listaDTO = unaLogica.ListaUsuarios();

            //Filtramos solo los Usuarios habilitados y de paso la convertimos a List<UsuarioModel>
            List<UsuarioModel> lista = HerramientasM.UsuariosHabilitados(listaDTO);

            foreach (UsuarioModel unUsuario in lista)
            {
                //Agregamos los Usuarios habilitados a la lista
                resultado.Add(new SelectListItem
                {
                    Text = (unUsuario.Usuario),
                    Value = unUsuario.Usuario.ToString(),
                    Selected = false
                });
            }

            //Retornamos la lista
            return resultado;
        }
    }
}