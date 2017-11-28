using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacturacionBOOSTRAP.Models
{
    public class ProductoModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Marca { get; set; }

        [Required]
        public decimal Precio { get; set; }
        
        public string NickUsuario { get; set; }
    }
}