using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class ClienteDTO
    {
        //Cedula del cliente
        public int Cedula { get; set; }
        
        //Nombre del cliente
        public string Nombre { get; set; }
        
        //Domicilio del cliente
        public string Domicilio { get; set; }
        
        //Fecha de nacimiento
        public DateTime FechaNacimiento { get; set; }
        
        //Usuario que registra al cliente, a la hora de registrar el cliente se completa automaticamente
        public string NickUsuario { get; set; }

        //Sera true si el cliente esta habilitado
        public bool Habilitado { get; set; }

    }
}
