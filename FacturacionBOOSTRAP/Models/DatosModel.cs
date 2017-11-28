using CL.Logicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturacionBOOSTRAP.Models
{
    public class DatosModel
    {
        /*En este modelo cargaremos los datos utiles que le puedan interesar al usuario*/

        /********************
         * DATOS DE FACTURAS*
         ********************/

        //Suma del total de todas las facturas
        public decimal TotalFacturado { get; set; }

        //Cantidad de Facturas registradas
        public int FacturasRegistradas { get; set; }

        /*******************
         *DATOS DE CLIENTES*
         *******************/
        public int TotalClientesRegistrados { get; set; }

        /********************
         *DATOS DE PRODUCTOS*
         ********************/
        public int ProductosRegistrados { get; set; }

        /*******************
         *DATOS DE USUARIOS*
         *******************/
        public int UsuariosRegistrados { get; set; }


        //Creamos un constructor que cargara toda la informacion cuando instanciemos este modelo
        public DatosModel()
        {
            //Instanciamos las logicas que necesitaremos
            LogicaProducto logicaProducto = new LogicaProducto();
            LogicaFactura logicaFactura = new LogicaFactura();
            LogicaCliente logicaCliente = new LogicaCliente();
            LogicaUsuario logicaUsuario = new LogicaUsuario();

            //Vamos completando las distintas propiedades del modelo
            //FACTURA:
            TotalFacturado = logicaFactura.TotalFacturado();
            FacturasRegistradas = logicaFactura.FacturasRegistradas();

            //CLIENTE:
            TotalClientesRegistrados = logicaCliente.TotalClientesRegistrados();

            //USUARIO
            UsuariosRegistrados = logicaUsuario.UsuariosRegistrados();

            //PRODUCTO
            ProductosRegistrados = logicaProducto.ProductosRegistrados();

        }
    }
}