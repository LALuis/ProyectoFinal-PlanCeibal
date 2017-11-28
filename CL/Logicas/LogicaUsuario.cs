using CD.Repositorio;
using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CL.Logicas
{
    public class LogicaUsuario
    {
        RepoUsuario miRepo = new RepoUsuario();

        //Lista de Usuarios
        public List<UsuarioDTO> ListaUsuarios()
        {
            return miRepo.ListaUsuarios();
        }

        //Crear Usuario
        public void CrearUsuario(UsuarioDTO usr)
        {
            miRepo.CrearUsuario(usr);
        }

        //Eliminar Usuario
        public void BorrarUsuario(string usr)
        {
            miRepo.BorrarUsuario(usr);
        }

        //Editar Usuario
        public void ActualizarUsuario(UsuarioDTO usr)
        {
            miRepo.ActualizarUsuario(usr);
        }

        //BuscarUsuario
        public UsuarioDTO BuscarUsuario(string id)
        {
            return miRepo.BuscarUsuario(id);
        }

        //Habilitar usuario
        public void HabilitarUsuario(string usr)
        {
            miRepo.HabilitarUsuario(usr);
        }

        /*DATOS*/
        public int UsuariosRegistrados()
        {
            return miRepo.UsuariosRegistrados();
        }
    }
}
