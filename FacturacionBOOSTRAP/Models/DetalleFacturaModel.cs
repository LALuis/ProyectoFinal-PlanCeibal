using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturacionBOOSTRAP.Models
{
    public class DetalleFacturaModel
    {
        //Cantidad de productos
        public int Cantidad { get; set; }

        //Subtotal es Precio de producto * cantidad
        public decimal SubTotal { get; set; }

        //Id del producto que agregaremos a la factura
        public int IdProducto { get; set; }

        //Nombre y marca del producto para mostrar en el detalle de la factura
        public string NombreMarca { get; set; }
    }
}