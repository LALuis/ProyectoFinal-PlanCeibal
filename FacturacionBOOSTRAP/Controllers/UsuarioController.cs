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
    public class UsuarioController : Controller
    {
        //Instanciamos nuestra logica de usuarios
        LogicaUsuario unaLogica = new LogicaUsuario();

        //GET: De Crear Usuario
        public ActionResult CrearUsuario()
        {
            //Controlamos que el usuario este logueado y sea admin
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                return View();
            }
            else //Si el usuario no se logueo
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //POST: Crear Usuario me llega como parametro
        [HttpPost]
        public ActionResult CrearUsuario(UsuarioModel unUsuario)
        {
            //Si el modelo NO es valido
            if (!ModelState.IsValid)
            {
                //Retornamos la vista con el error
                return View(unUsuario);
            }
            else //Si el modelo es correcto
            {
                //Nos anticipamos a un posible error en nuestra base de datos
                try
                {
                    //Controlamos que el id de usuario no este en uso
                    UsuarioDTO buscar = unaLogica.BuscarUsuario(unUsuario.Usuario);
                    if (buscar != null)
                    {
                        //Creamos el error en el campo usuario
                        ModelState.AddModelError("Usuario", $"El usuario {unUsuario.Usuario} ya esta en uso");
                        //Retornamos la vista con el error
                        return View(unUsuario);
                    }
                    else //si el usuario no esta en uso
                    {
                        //Controlamos la contraseña.
                        /*La contraseña debe tener entre 8 y 16 caracteres, mayusculas, minusculas
                         y simbolos*/
                        if (FuncionesAuxiliares.ContrasenaEsValida(unUsuario.Contrasena))
                        {
                            //Pasamos nuestro usuarioModel a usuarioDTO
                            UsuarioDTO nuevoUsuario = HerramientasM.ConvertirUsuarioModel(unUsuario);

                            //enviamos nuestro usuario a la logica
                            unaLogica.CrearUsuario(nuevoUsuario);

                            //redirigimos al GET para que se reseteen los campos del formulario
                            return RedirectToAction("CrearUsuario");
                        }
                        else //si la contraseña no es segura
                        {
                            //Creamos el mensaje de error
                            ModelState.AddModelError("Contrasena", $"La contraseña debe tener: minusculas, mayusculas, numeros, simbolos y entre 8 y 16 caracteres sin espacios en blanco");
                            //Devolvemos la vista con el error
                            return View(unUsuario);
                        }
                    }
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base de datos
                    //Retornamos la vista de Error
                    return View("Error");
                }
            }
        }

        //GET: Eliminar Usuario(Mostrara el detalle del usuario que queremos eliminar)
        public ActionResult EliminarUsuario(string Usuario)
        {
            //Controlamos que el usuario este logueado y sea admin
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Anticipamos un posible error en servidor
                try
                {
                    //Buscamos el usuario en la base de datos
                    UsuarioDTO unUsuario = unaLogica.BuscarUsuario(Usuario);
                    //Convertimos el usuarioDTO a usuarioModel y lo pasamos a la vista
                    return View(HerramientasM.ConvertirUsuarioDTO(unUsuario));
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

        //Action de confirmacion de la eliminacion del Usuario
        public ActionResult ConfirmaEliminarUsuario(string Usuario)
        {
            //Controlamos que el usuario este logueado y sea admin
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Verificamos que la id no venga nula, aunque si es seleccionada de la lista siempre tendra un valor valido
                if (!string.IsNullOrEmpty(Usuario))
                {
                    //Anticipamos un posible error en el servidor
                    try
                    {
                        unaLogica.BorrarUsuario(Usuario);
                        return RedirectToAction("MostrarUsuarios", "Usuario");
                    }
                    catch (Exception unError)
                    {
                        //Podriamos guardar el error en la base
                        //Redirigimos a vista de error
                        return View("Error");
                    }
                }
                else
                {
                    //Si el usuario que nos llega esta vacio, redirigimos a la siguiente vista
                    return RedirectToAction("MostrarUsuarios");
                }
            }
            else //Si el usuario no se logueo o no es admin
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //Mostrar Usuarios:
        public ActionResult MostrarUsuarios()
        {
            //Controlamos que el usuario este logueado y sea administrador:
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Anticipamos a un posible error en la base de datos
                try
                {
                    //Controlar que la lista no sea nula, pero lo hacemos dentro de la vista porque debemos mostrar algo si es nula
                    List<UsuarioDTO> listaUsuariosDTO = unaLogica.ListaUsuarios();

                    //Seleccionamos de la lista solo los usuarios habilitados
                    List<UsuarioModel> lista = HerramientasM.UsuariosHabilitados(listaUsuariosDTO);

                    //Pasamos la lista de usuarios habilitados a la vista
                    return View(lista);
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
            else //Si no esta logueado o no es administrador enviamos al login
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //Mostrar Usuarios Deshabilitados:
        public ActionResult MostrarUsuariosDeshabilitados()
        {
            //Controlamos que el usuario este logueado y sea administrador:
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Anticipamos a un posible error en la base de datos
                try
                {
                    //Controlar que la lista no sea nula, pero lo hacemos dentro de la vista porque debemos mostrar algo si es nula
                    List<UsuarioDTO> listaUsuariosDTO = unaLogica.ListaUsuarios();

                    //Seleccionamos de la lista solo los usuarios deshabilitados
                    List<UsuarioModel> lista = HerramientasM.UsuariosDeshabilitados(listaUsuariosDTO);

                    //Pasamos la lista de usuarios deshabilitados a la vista
                    return View(lista);
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
            else //Si no esta logueado o no es administrador enviamos al login
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //Mostraremos el detalle de un usuario
        public ActionResult MostrarUsuario(string Usuario)
        {
            //Controlamos que el usuario este logueado y sea administrador:
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Anticipamos un posible error en la base de datos
                try
                {
                    //Buscamos el usuario en la base de datos
                    //lo convertimos a UsuarioModel con nuestro metodo en HerramientasM y lo pasamos a la vista
                    return View(HerramientasM.ConvertirUsuarioDTO(unaLogica.BuscarUsuario(Usuario)));
                }
                catch(Exception unError)
                {
                    //Podriamos guardar el error en nuestra base de datos
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
            else //Si no esta logueado o no es administrador
            {
                //Redirigimos al login
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //EditarUsuarios:
        public ActionResult EditarUsuario(string Usuario)
        {
            //Controlamos que el usuario este logueado y sea administrador:
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Prevemos un posible error en nuestro servidor
                try
                {
                    //Buscamos el usuario en la base
                    UsuarioDTO miUsuario = unaLogica.BuscarUsuario(Usuario);

                    //Convertimos el usuarioDTO a UsuarioModel con el metodo que creamos en HerramientasM y se lo pasamos a la vista
                    return View(HerramientasM.ConvertirUsuarioDTO(miUsuario));
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Redirigimos a la vista de error
                    return View("Error");
                }

            }
            else //Si no esta logueado o no es administrador enviamos al login
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //POST: De EditarUsuario
        [HttpPost]
        public ActionResult EditarUsuario(UsuarioModel unUsuario)
        {
            //Controlamos que el modelo sea valido
            if (!ModelState.IsValid)
            {
                //El modelo no es valido y mostramos el error
                return View(unUsuario);
            }
            else //El modelo es valido
            {
                //Anticipamos un posible error en base
                try
                {
                    //Controlamos la contraseña.
                    /*La contraseña debe tener entre 8 y 16 caracteres, mayusculas, minusculas
                     y simbolos*/
                    if (FuncionesAuxiliares.ContrasenaEsValida(unUsuario.Contrasena))
                    {
                        //Convertimos nuestro usuarioModel a UsuarioDTO
                        UsuarioDTO miusuario = HerramientasM.ConvertirUsuarioModel(unUsuario);
                        //Pasamos el usuario a la logica
                        unaLogica.ActualizarUsuario(miusuario);
                        //Luego de terminado devolvemos la vista de usuarios
                        return RedirectToAction("MostrarUsuarios");
                    }
                    else //si la contraseña no es segura
                    {
                        //Creamos el mensaje de error
                        ModelState.AddModelError("Contrasena", $"La contraseña debe tener: minusculas, mayusculas, numeros, simbolos y entre 8 y 16 caracteres sin espacios en blanco");
                        //Devolvemos la vista con el error
                        return View(unUsuario);
                    }
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Redirigimos a la vista de error
                    return View("Error");
                }
            }
        }

        public ActionResult HabilitarUsuario(string Usuario)
        {
            //Controlamos que el usuario este logueado y sea administrador:
            if (Session["Logueado"] != null && HerramientasM.EsAdmin((UsuarioModel)Session["Logueado"]))
            {
                //Prevemos un posible error en nuestro servidor
                try
                {
                    //Pasamos el usuario a la logica para que lo habilite
                    unaLogica.HabilitarUsuario(Usuario);

                    //Redirigimos a la accion con los usuarios deshabilitados
                    return RedirectToAction("MostrarUsuariosDeshabilitados");
                }
                catch (Exception unError)
                {
                    //Podriamos guardar el error en la base
                    //Redirigimos a la vista de error
                    return View("Error");
                }

            }
            else //Si no esta logueado o no es administrador enviamos al login
            {
                return RedirectToAction("Login", "Ingreso");
            }
        }

        //Este action recibe un id de usuario, devuelve una vista y llama a 3 PartialViewResult pasandole como parametro esa id
        //Esos partialviewResult buscaran el usuario en la base y mostraran las facturas, productos y clientes que registro
        public PartialViewResult HistorialUsuario(string Usuario)
        {
                return PartialView("_HistorialUsuario",Usuario);
        }
        
    }
}