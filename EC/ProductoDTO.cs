using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class ProductoDTO
    {
        //Id del producto que ingresa el usuario
        public int Id { get; set; }
        
        //Nombre del producto
        public string Nombre { get; set; }
        
        //Marca
        public string Marca { get; set; }

        //Precio unitario
        public decimal Precio { get; set; }
        
        //Nick del usuario que lo creo, a la hora de crearlo se completa automaticamente
        public string NickUsuario { get; set; }

        //si el usuario esta habilitado sera true
        public bool Habilitado { get; set; }
    }
}
