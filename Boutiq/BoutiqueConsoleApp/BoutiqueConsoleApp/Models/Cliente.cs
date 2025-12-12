using System;

namespace BoutiqueConsoleApp.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Constructor vacío
        public Cliente() { }

        // Constructor con parámetros
        public Cliente(string nombre, string apellido, string telefono, string email, DateTime? fechaNacimiento)
        {
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Email = email;
            FechaNacimiento = fechaNacimiento;
            FechaRegistro = DateTime.Now;
        }

        // Propiedad calculada: Nombre completo
        public string NombreCompleto => $"{Nombre} {Apellido}";

        // Método para calcular edad
        public int? CalcularEdad()
        {
            if (!FechaNacimiento.HasValue)
                return null;

            int edad = DateTime.Now.Year - FechaNacimiento.Value.Year;
            if (DateTime.Now.DayOfYear < FechaNacimiento.Value.DayOfYear)
                edad--;

            return edad;
        }

        public override string ToString()
        {
            return $"ID: {ClienteID} | {NombreCompleto} | Tel: {Telefono} | Email: {Email}";
        }
    }
}