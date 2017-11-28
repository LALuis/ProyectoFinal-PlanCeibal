using CD.ModeloBase;
using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD.Repositorio
{
    public class RepoFactura
    {
        public List<FacturaDTO> ListaFacturas()
        {
            //creamos la lista resultante
            List<FacturaDTO> resultado = new List<FacturaDTO>();

            using (BaseSistema miBase = new BaseSistema())
            {
                //Obtenemos la lista de facturas y el detalle al mismo tiempo
                foreach(Factura f in miBase.Factura)
                {
                    //Creamos la facturaDTO y le asignamos los valores de la Factura de la base
                    FacturaDTO miFactura = new FacturaDTO()
                    {
                        numeroFactura = f.numeroFactura,
                        NumeroCliente = f.numeroCliente,
                        Fecha = f.fecha,
                        NickUsuario = f.nickUsuario,
                        Total = f.total,
                        ListaDetalle = new List<DetalleFacturaDTO>()
                    };

                    //Ahora que tenemos una factura creada le agregamos su detalle, buscando en la lista de detalles
                    foreach(DetallesFactura d in miBase.DetallesFactura)
                    {
                        //Buscamos los detalles que correspondan a la factura actual
                        if(miFactura.numeroFactura == d.factura)
                        {
                            //Creamos un nuevo detalle con los datos que corresponden
                            DetalleFacturaDTO unDetalle = new DetalleFacturaDTO()
                            {
                                Cantidad = d.cantidad,
                                IdFactura = d.factura,
                                IdProducto = d.idProducto,
                                SubTotal = d.subTotal
                            };
                            //Agregamos el detalle a la factura:
                            miFactura.ListaDetalle.Add(unDetalle);
                        } 
                    }
                    //Agregamos (luego de ya haber agregado los datos y los detalles) la factura a la lista resultado
                    resultado.Add(miFactura);
                }
            }
            //retornamos la lista resultado que contiene la lista de facturas de la base de datos
            return resultado;
        }

        public void AgregarFactura(FacturaDTO miFactura)
        {
            /*Creamos el principio de la factura con los datos que podemos agregar*/
            Factura nuevaFactura = new Factura()
            {
                fecha = miFactura.Fecha,
                numeroCliente = miFactura.NumeroCliente,
                total = miFactura.Total,
                nickUsuario = miFactura.NickUsuario
            };

            using (BaseSistema miBase = new BaseSistema())
            {
                miBase.Factura.Add(nuevaFactura);
                miBase.SaveChanges();

                foreach(var item in miFactura.ListaDetalle)
                {
                    /*Creamos detalle por detalle y lo vamos agregando*/
                    DetallesFactura nuevoDetalle = new DetallesFactura()
                    {
                        cantidad = item.Cantidad,
                        idProducto = item.IdProducto,
                        subTotal = item.SubTotal,

                        /*El id de la factura es el mismo y se genero automatico cuando agregue la factura*/
                        factura = nuevaFactura.numeroFactura
                    };
                    miBase.DetallesFactura.Add(nuevoDetalle);
                }

                /*Guardamos los cambios*/
                miBase.SaveChanges();
            }
        }

        public FacturaDTO BuscarFactura(int numeroFactura)
        {
            //Creamos el resultado como null, luego si encontramos una factura le asignamos su valor
            FacturaDTO resultado = null; 

            using (BaseSistema miBase = new BaseSistema())
            {
                //Obtenemos la lista de facturas y el detalle al mismo tiempo
                foreach (Factura f in miBase.Factura)
                {
                    //Buscamos la factura con el mismo id
                    if (f.numeroFactura == numeroFactura)
                    {
                        resultado = new FacturaDTO()
                        {
                            numeroFactura = f.numeroFactura,
                            NumeroCliente = f.numeroCliente,
                            Fecha = f.fecha,
                            NickUsuario = f.nickUsuario,
                            Total = f.total,
                            ListaDetalle = new List<DetalleFacturaDTO>()
                        };

                        //Ahora que tenemos una factura creada le agregamos su detalle, buscando en la lista de detalles
                        foreach (DetallesFactura d in miBase.DetallesFactura)
                        {
                            //Buscamos los detalles que correspondan a la factura actual
                            if (resultado.numeroFactura == d.factura)
                            {
                                //Creamos un nuevo detalle con los datos que corresponden
                                DetalleFacturaDTO unDetalle = new DetalleFacturaDTO()
                                {
                                    Cantidad = d.cantidad,
                                    IdFactura = d.factura,
                                    IdProducto = d.idProducto,
                                    SubTotal = d.subTotal
                                };
                                //Agregamos el detalle a la factura:
                                resultado.ListaDetalle.Add(unDetalle);
                            }
                        }
                        //Retornamos la faltura terminando el ciclo, porque ya no necesitamos buscar mas
                        return resultado;
                    }
                }
                //En caso de no encontrar ninguna se retornara null porque asi definimos resultado al principio:
                return resultado;
            }
        }

        public List<FacturaDTO> FacturasUsuario(string usuario)
        {
            //creamos la lista resultante
            List<FacturaDTO> resultado = new List<FacturaDTO>();

            using (BaseSistema miBase = new BaseSistema())
            {
                //Obtenemos la lista de facturas y el detalle al mismo tiempo
                foreach (Factura f in miBase.Factura)
                {
                    //Verificamos que el usuario sea el que nos pasan
                    if(f.nickUsuario.Trim().ToLower() == usuario.Trim().ToLower())
                    {
                        //Creamos la facturaDTO y le asignamos los valores de la Factura de la base
                        FacturaDTO miFactura = new FacturaDTO()
                        {
                            numeroFactura = f.numeroFactura,
                            NumeroCliente = f.numeroCliente,
                            Fecha = f.fecha,
                            NickUsuario = f.nickUsuario,
                            Total = f.total,
                            ListaDetalle = new List<DetalleFacturaDTO>()
                        };

                        //Ahora que tenemos una factura creada le agregamos su detalle, buscando en la lista de detalles
                        foreach (DetallesFactura d in miBase.DetallesFactura)
                        {
                            //Buscamos los detalles que correspondan a la factura actual
                            if (miFactura.numeroFactura == d.factura)
                            {
                                //Creamos un nuevo detalle con los datos que corresponden
                                DetalleFacturaDTO unDetalle = new DetalleFacturaDTO()
                                {
                                    Cantidad = d.cantidad,
                                    IdFactura = d.factura,
                                    IdProducto = d.idProducto,
                                    SubTotal = d.subTotal
                                };
                                //Agregamos el detalle a la factura:
                                miFactura.ListaDetalle.Add(unDetalle);
                            }
                        }
                        //Agregamos (luego de ya haber agregado los datos y los detalles) la factura a la lista resultado
                        resultado.Add(miFactura);
                    }
                }
            }
            //retornamos la lista resultado que contiene la lista de facturas de la base de datos
            return resultado;
        }

        /*****************
         *    DATOS      *
         *****************/
         
        //Retorna la suma de los totales de todas las facturas
        public decimal TotalFacturado()
        {
            //Instanciamos nuestra base de datos
            using(BaseSistema miBase = new BaseSistema())
            {
                //Calculamos la suma de todos los totales
                var res = miBase.Factura.Sum(m => m.total);

                //Retornamos el total
                return res;
            }
        }

        //Retorna la cantidad de facturas registradas
        public int FacturasRegistradas()
        {
            //Instanciamos nuestra base de datos
            using (BaseSistema miBase = new BaseSistema())
            {
                //Consultamos la cantidad de facturas registradas
                var res = miBase.Factura.Count();

                //Retornamos esa cantidad
                return res;
            }
        }
    }
}
