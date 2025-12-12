using System;

namespace BoutiqueConsoleApp.Models
{
    public class Descuento
    {
        public int DescuentoID { get; set; }
        public string Nombre { get; set; }
        public decimal Porcentaje { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }

        public Descuento() { }

        public Descuento(string nombre, decimal porcentaje, DateTime fechaInicio, DateTime fechaFin)
        {
            Nombre = nombre;
            Porcentaje = porcentaje;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Activo = true;
        }

        // Verificar si el descuento está vigente
        public bool EstaVigente()
        {
            DateTime hoy = DateTime.Now.Date;
            return Activo && hoy >= FechaInicio.Date && hoy <= FechaFin.Date;
        }

        // Calcular descuento sobre un monto
        public decimal CalcularDescuento(decimal monto)
        {
            if (!EstaVigente())
                return 0;

            return monto * (Porcentaje / 100);
        }

        public override string ToString()
        {
            string estado = EstaVigente() ? "VIGENTE" : "NO VIGENTE";
            return $"{Nombre} - {Porcentaje}% [{estado}] ({FechaInicio:dd/MM/yyyy} - {FechaFin:dd/MM/yyyy})";
        }
    }
}