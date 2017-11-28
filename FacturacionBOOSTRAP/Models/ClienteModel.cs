using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacturacionBOOSTRAP.Models
{
    public class ClienteModel
    {
        [Required]
        public int Cedula { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Domicilio { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        /*Se especifica en el controlador, no lo ingresa el usuario*/
        public string NickUsuario { get; set; }
    }
}