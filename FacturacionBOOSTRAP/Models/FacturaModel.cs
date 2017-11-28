using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacturacionBOOSTRAP.Models
{
    public class FacturaModel
    {
        //Se genera automatico, sin embargo lo agregamos para la hora de listar
        public int NumeroFactura { get; set; }

        //Se carga automaticamente, sin embargo lo agregamos para la hora de listar
        public DateTime Fecha { get; set; }
        //Se carga automaticamente, sin embargo lo agregamos para la hora de listar
        public string NickUsuario { get; set; }


        public int NumeroCliente { get; set; }
        
        //IdProducto nos servira para agregar productos a nuestra factura
        public int IdProducto { get; set; }
        //Cantidad nos servira para saber cuantos productos de la id anterior debemos agregar
        [Range(minimum:1,maximum:10000000, ErrorMessage = "Debe ingresar una cantidad valida entre 1 y 10000000")]
        public int Cantidad { get; set; }

        //Lista de productos que se pueden seleccionar
        public List<SelectListItem> ListaProductos { get; set; }
        //Lista de Clientes que se pueden seleccionar
        public List<SelectListItem> ListaClientes { get; set; }

        //Nick usuario lo omitimos porque lo agregamos con la session

        public List<DetalleFacturaModel> ListaDetalle { get; set; }

        public decimal Total { get; set; }

        //Nombre del cliente para mostrar en el detalle de que cliente se trata
        public string NombreCliente { get; set; }
    }
}