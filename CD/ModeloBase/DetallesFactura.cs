namespace CD.ModeloBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetallesFactura")]
    public partial class DetallesFactura
    {
        public int id { get; set; }

        public int cantidad { get; set; }

        public decimal subTotal { get; set; }

        public int idProducto { get; set; }

        public int factura { get; set; }

        public virtual Factura Factura1 { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
