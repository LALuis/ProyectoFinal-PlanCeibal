using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class FacturaDTO
    {
        //Numero de la factura que a la hora de registrarla se genera automaticamente
        public int numeroFactura { get; set; }

        //Fecha de creacion, a la hora de registrar la factura tambien se genera automaticamente
        public DateTime Fecha { get; set; }

        //Numero de cliente
        public int NumeroCliente { get; set; }

        //Usuario que crea la factura, a la hora de registrarla, tambien se completa automaticamente
        public string NickUsuario { get; set; }

        //Lista de detalles, es una lista de cantidad de x producto
        public List<DetalleFacturaDTO> ListaDetalle { get; set; }

        //La suma de La cantidad * el precio de la lista de detalles
        public decimal Total { get; set; }
    }
}
