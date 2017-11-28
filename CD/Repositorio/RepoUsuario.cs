using CD.ModeloBase;
using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD.Repositorio
{
    public class RepoUsuario
    {
        //Devolver Lista
        public List<UsuarioDTO> ListaUsuarios()
        {
            /*Retorna una lista de usuarios, esten habilitados o no*/

            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Para cada usuario en la base creamos un usuarioDTO con los mismos valores:
                var listaResultante = from usuario
                            in miBase.Usuario
                                      select new UsuarioDTO
                                      {
                                          Contrasena = usuario.Contrasena,
                                          EsAdmin = usuario.EsAdmin,
                                          Usuario = usuario.Usuario1,
                                          Habilitado = usuario.habilitado
                                      };

                //Retornamos la lista que acabamos de crear:
                return listaResultante.ToList();
            }
        }

        //Crear
        public void CrearUsuario(UsuarioDTO usr)
        {
            //Instanciamos la base de datos:
            using (BaseSistema miBase = new BaseSistema())
            {
                //Creamos el usuario
                Usuario nuevoUsuario = new Usuario()
                {
                    //Mapeamos la tabla, agregamos uno a uno los datos:
                    Usuario1 = usr.Usuario,
                    Contrasena = usr.Contrasena,
                    EsAdmin = usr.EsAdmin,
                    //Como estamos creando un usuario este esta habilitado
                    habilitado = true
                };
                //Agregamos el usuario
                miBase.Usuario.Add(nuevoUsuario);
                //Hacemos los cambios en la base de datos:
                miBase.SaveChanges();
            }
        }

        //Eliminar
        public void BorrarUsuario(string usr)
        {
            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //EL USUARIO A ELIMINAR NO PUEDE SER EL SUPERADMIN
                if (usr.Trim().ToLower() != "superadmin")
                {
                    //Buscamos el usuario
                    var usuarioBorrar = (from usuario in miBase.Usuario where usuario.Usuario1.Trim().ToLower() == usr.Trim().ToLower() select usuario).First();

                    //Fijamos el usuario
                    miBase.Usuario.Attach(usuarioBorrar);
                    //Cambiamos habilitado a false
                    usuarioBorrar.habilitado = false;
                    //Guardamos los cambios
                    miBase.SaveChanges();
                }
            }
        }

        //Habilitar
        public void HabilitarUsuario(string usr)
        {
            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos el usuario
                var usuarioBorrar = (from usuario in miBase.Usuario where usuario.Usuario1.Trim().ToLower() == usr.Trim().ToLower() select usuario).First();

                //Fijamos el usuario
                miBase.Usuario.Attach(usuarioBorrar);
                //Cambiamos habilitado a true
                usuarioBorrar.habilitado = true;
                //Guardamos los cambios
                miBase.SaveChanges();
            }
        }


        //Editar
        public void ActualizarUsuario(UsuarioDTO usr)
        {
            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //EL USUARIO A EDITAR NO PUEDE SER EL SUPERADMIN
                if(usr.Usuario.Trim().ToLower() != "superadmin")
                {
                    //Buscamos el usuario en la base de datos
                    var usrActualizar = miBase.Usuario.Where(p => p.Usuario1 == usr.Usuario).FirstOrDefault();

                    //Fijamos el usuario
                    miBase.Usuario.Attach(usrActualizar);

                    //Mapeamos sus datos
                    usrActualizar.Usuario1 = usr.Usuario;
                    usrActualizar.Contrasena = usr.Contrasena;
                    usrActualizar.EsAdmin = usr.EsAdmin;
                    //Solo editamos usuarios habilitados
                    usrActualizar.habilitado = true;

                    //Guardamos los cambios
                    miBase.SaveChanges();
                }
            }
        }

        public UsuarioDTO BuscarUsuario(string id)
        {
            /*Retorna un usuario, no importa si esta habilitado o no*/

            //Creamos el usuario resultante
            UsuarioDTO miUsuario;

            //Instanciamos la base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Buscamos el usuario en la base de datos
                var usuarioBase = miBase.Usuario.Where(usr => usr.Usuario1.Trim().ToLower() == id.Trim().ToLower()).FirstOrDefault();

                //Si no encontramos el usuario retornamos null:
                if (usuarioBase == null)
                {
                    miUsuario = null;
                }
                //Si encontramos el usuario hacemos el mapping correspondiente para pasarlo como UsuarioDTO:
                else
                {
                    //Creamos el usuario que devolveremos
                    miUsuario = new UsuarioDTO()
                    {
                        Usuario = usuarioBase.Usuario1,
                        Contrasena = usuarioBase.Contrasena,
                        EsAdmin = usuarioBase.EsAdmin,
                        Habilitado = usuarioBase.habilitado
                        
                    };
                }
                //Retornamos el usuario resultante
                return miUsuario;
            }

        }

        /*DATOS*/
        public int UsuariosRegistrados()
        {
            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Consultamos la cantidad de usuarios registrados
                var res = miBase.Usuario.Count();

                //Devolvemos esa cantidad
                return res;
            }
        }
    }
}
