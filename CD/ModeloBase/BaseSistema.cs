namespace CD.ModeloBase
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BaseSistema : DbContext
    {
        public BaseSistema()
            : base("name=BaseSistema")
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<DetallesFactura> DetallesFactura { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.domicilio)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.nickUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Factura)
                .WithRequired(e => e.Cliente)
                .HasForeignKey(e => e.numeroCliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetallesFactura>()
                .Property(e => e.subTotal)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Factura>()
                .Property(e => e.total)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Factura>()
                .Property(e => e.nickUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Factura>()
                .HasMany(e => e.DetallesFactura)
                .WithRequired(e => e.Factura1)
                .HasForeignKey(e => e.factura)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Producto>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Producto>()
                .Property(e => e.marca)
                .IsUnicode(false);

            modelBuilder.Entity<Producto>()
                .Property(e => e.precio)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Producto>()
                .Property(e => e.nickUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Producto>()
                .HasMany(e => e.DetallesFactura)
                .WithRequired(e => e.Producto)
                .HasForeignKey(e => e.idProducto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Usuario1)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Contrasena)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Cliente)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.nickUsuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Factura)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.nickUsuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Producto)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.nickUsuario)
                .WillCascadeOnDelete(false);
        }
    }
}
