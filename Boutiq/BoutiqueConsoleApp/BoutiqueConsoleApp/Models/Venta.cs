using System;
using System.Collections.Generic;

namespace BoutiqueConsoleApp.Models
{
    public class Venta
    {
        public int VentaID { get; set; }
        public int ClienteID { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }

        // Propiedades de navegación
        public string NombreCliente { get; set; }
        public List<DetalleVenta> Detalles { get; set; }

        // Constructor
        public Venta()
        {
            FechaVenta = DateTime.Now;
            Detalles = new List<DetalleVenta>();
        }

        // Calcular totales
        public void CalcularTotales()
        {
            Subtotal = 0;
            foreach (var detalle in Detalles)
            {
                Subtotal += detalle.Subtotal;
            }
            Total = Subtotal - Descuento;
        }

        public override string ToString()
        {
            return $"Venta #{VentaID} | Cliente: {NombreCliente} | Total: ${Total:F2} | {FechaVenta:dd/MM/yyyy}";
        }
    }

    public class DetalleVenta
    {
        public int DetalleID { get; set; }
        public int VentaID { get; set; }
        public int PrendaID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        // Propiedades de navegación
        public string NombrePrenda { get; set; }

        public DetalleVenta()
        {
        }

        public DetalleVenta(int prendaID, string nombrePrenda, int cantidad, decimal precioUnitario)
        {
            PrendaID = prendaID;
            NombrePrenda = nombrePrenda;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Subtotal = cantidad * precioUnitario;
        }

        public override string ToString()
        {
            return $"{NombrePrenda} x{Cantidad} - ${PrecioUnitario:F2} c/u = ${Subtotal:F2}";
        }
    }
}