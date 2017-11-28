using CD.Repositorio;
using EC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Logicas
{
    public class LogicaFactura
    {
        /*Instanciamos el repositorio de facturas*/
        RepoFactura repositorioFactura = new RepoFactura();

        /*Registramos la factura que nos llega como parametro*/
        public void AgregarFactura(FacturaDTO miFactura)
        {
            repositorioFactura.AgregarFactura(miFactura);
        }

        /*Retornamos la lista de todas las facturas registradas en el sistema*/
        public List<FacturaDTO> ListaFacturas()
        {
            return repositorioFactura.ListaFacturas();
        }

        /*Buscamos y retornamos la factura cuyo numero es el mismo que nos pasan como parametro*/
        public FacturaDTO BuscarFactura(int numeroFactura)
        {
            return repositorioFactura.BuscarFactura(numeroFactura);
        }

        /*Retornamos la lista de facturas registradas por el usuario que nos pasan como parametro*/
        public List<FacturaDTO> FacturasUsuario(string usuario)
        {
            return repositorioFactura.FacturasUsuario(usuario);
        }

        /*DATOS*/
        //Retorna la suma de los totales de todas las facturas
        public decimal TotalFacturado()
        {
            return repositorioFactura.TotalFacturado();
        }

        //Retorna la cantidad de facturas registradas
        public int FacturasRegistradas()
        {
            return repositorioFactura.FacturasRegistradas();
        }
    }
}
