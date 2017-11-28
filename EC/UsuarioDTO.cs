using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class UsuarioDTO
    {
        //Nombre de usuario
        public string Usuario { get; set; }

        //Contraseña
        public string Contrasena { get; set; }

        //Si el usuario es admin sera true
        public bool EsAdmin { get; set; }

        //Si el usuario esta habilitado sera true
        public bool Habilitado { get; set; }
    }
}
