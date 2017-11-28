using CL.Herramientas;
using CL.Logicas;
using EC;
using FacturacionBOOSTRAP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacturacionBOOSTRAP.Controllers
{
    public class IngresoController : Controller
    {
        //Creamos una instancia de la Logica de la cual podremos acceder desde todos los Action
        LogicaUsuario miLogica = new LogicaUsuario();

        // GET: Ingreso
        public ActionResult Login()
        {
            //Retornamos la vista de ingreso
            return View();
        }

        //POST: Ingreso
        [HttpPost]
        public ActionResult Login(UsuarioModel unUsuario)
        {
            //Manejamos un posible error inesperado
            try
            {
                //Verificamos que el modelo sea valido
                if (!ModelState.IsValid)
                {
                    //Si el modelo no es valido retornamos la vista con el error
                    return View(unUsuario);
                }
                else
                {
                    //Buscamos el usuario en la base de datos
                    UsuarioDTO usuarioActual = miLogica.BuscarUsuario(unUsuario.Usuario);

                    //Verificamos los datos y operamos segun corresponda
                    if(usuarioActual == null)
                    {
                        //Si el usuario es null quiere decir que no se encontro el usuario que se ingreso
                        //Creamos el mensaje de error correspondiente
                        ModelState.AddModelError("", $"No se encontro el usuario {unUsuario.Usuario}");

                        //Pasamos el usuario al modelo
                        return View(unUsuario);
                    }
                    else if (!usuarioActual.Habilitado)//El usuario no esta habilitado
                    {
                        //Si el usuario no esta habilitado quiere decir que fue borrado
                        //Creamos el mensaje de error correspondiente
                        ModelState.AddModelError("", $"El usuario {unUsuario.Usuario} no esta habilitado");

                        //Pasamos el usuario al modelo
                        return View(unUsuario);
                    }
                    else
                    {
                        /*Puede ser admin o usuario*/
                        if (FuncionesAuxiliares.EsAdmin(usuarioActual) && unUsuario.Contrasena == usuarioActual.Contrasena)
                        {
                            //Guardamos en sesion como Logueado, creamos un nuevo usuarioModel con los datos de usuarioActual y lo guardamos...
                            Session.Add("Logueado", new UsuarioModel() { Usuario = usuarioActual.Usuario, Contrasena = usuarioActual.Contrasena, EsAdmin = usuarioActual.EsAdmin });

                            //Redirigimos a la vista principal de Admin
                            return RedirectToAction("Index", "Home");
                        }
                        else if(!FuncionesAuxiliares.EsAdmin(usuarioActual) && unUsuario.Contrasena == usuarioActual.Contrasena)
                        {
                            //Guardamos en sesion como Logueado, creamos un nuevo usuarioModel con los datos de usuarioActual y lo guardamos...
                            Session.Add("Logueado", new UsuarioModel() { Usuario = usuarioActual.Usuario, Contrasena = usuarioActual.Contrasena, EsAdmin = usuarioActual.EsAdmin });

                            //Redirigimos a la vista principal de usuario
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            //Estamos ante una contraseña incorrecta
                            //Creamos el mensaje de error correspondiente
                            ModelState.AddModelError("", $"Contraseña incorrecta!");

                            //Retornamos la vista con el error pasandole el modelo
                            return View(unUsuario);
                        }
                    }
                }
            }
            catch(Exception unError)
            {
                /*Podriamos guardar el error en la base de datos*/
                //Redirigimos a la vista de error de servidor
                return View("Error");
            }
        }

        //Creamos el logout
        public ActionResult Logout()
        {
            //Eliminamos la session de login
            Session.Remove("Logueado");

            //Redirigimos a la vista de login
            return RedirectToAction("Login");
        }
        
    }
}