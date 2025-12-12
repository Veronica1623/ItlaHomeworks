using System;

namespace BoutiqueConsoleApp.Models
{
    public class Prenda
    {
        public int PrendaID { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int? ProveedorID { get; set; }
        public string Temporada { get; set; }
        public DateTime FechaIngreso { get; set; }

        // Constructor vacío
        public Prenda() { }

        // Constructor con parámetros
        public Prenda(string nombre, string categoria, string talla, string color,
                      decimal precioCompra, decimal precioVenta, int stock,
                      int? proveedorID, string temporada)
        {
            Nombre = nombre;
            Categoria = categoria;
            Talla = talla;
            Color = color;
            PrecioCompra = precioCompra;
            PrecioVenta = precioVenta;
            Stock = stock;
            ProveedorID = proveedorID;
            Temporada = temporada;
            FechaIngreso = DateTime.Now;
        }

        // Método para mostrar información
        public override string ToString()
        {
            return $"ID: {PrendaID} | {Nombre} | {Categoria} | Talla: {Talla} | " +
                   $"Color: {Color} | Precio: ${PrecioVenta:F2} | Stock: {Stock}";
        }
    }
}