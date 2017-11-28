using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class DetalleFacturaDTO
    {
        //IdDetalle se genera automaticamente, por eso lo omitimos
        public int Cantidad { get; set; }

        //Representa la cantidad * el total
        public decimal SubTotal { get; set; }

        //id del producto
        public int IdProducto { get; set; }

        //Factura a la que corresponde este detalle
        public int IdFactura { get; set; }
    }
}
