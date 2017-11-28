namespace CD.ModeloBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Factura")]
    public partial class Factura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Factura()
        {
            DetallesFactura = new HashSet<DetallesFactura>();
        }

        [Key]
        public int numeroFactura { get; set; }

        public DateTime fecha { get; set; }

        public int numeroCliente { get; set; }

        public decimal total { get; set; }

        [Required]
        [StringLength(50)]
        public string nickUsuario { get; set; }

        public virtual Cliente Cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetallesFactura> DetallesFactura { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
