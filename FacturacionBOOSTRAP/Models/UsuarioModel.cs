using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacturacionBOOSTRAP.Models
{
    public class UsuarioModel
    {
        /*No se puede ingresar usuario vacio y este no puede tener mas de 50 caracteres*/
        [Required(AllowEmptyStrings = false, ErrorMessage ="Debe ingresar un Usuario valido")]
        public string Usuario { get; set; }

        /*No se pueden introducir caracteres nulos, el mensaje es el que busca razor y lo muestra en vista*/
        [Required(ErrorMessage ="Debe ingresar una contraseña valida")]
        public string Contrasena { get; set; }

        [Required]
        public bool EsAdmin { get; set; }

        //Lista de Usuarios que se pueden seleccionar
        public List<SelectListItem> ListaUsuarios { get; set; }
    }
}