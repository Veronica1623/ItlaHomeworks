using System;

namespace BoutiqueConsoleApp.Models
{
    public class Proveedor
    {
        public int ProveedorID { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Constructor vacío
        public Proveedor() { }

        // Constructor con parámetros
        public Proveedor(string nombre, string telefono, string email, string direccion)
        {
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
            Direccion = direccion;
            FechaRegistro = DateTime.Now;
        }

        public override string ToString()
        {
            return $"ID: {ProveedorID} | {Nombre} | Tel: {Telefono} | {Email}";
        }
    }
}