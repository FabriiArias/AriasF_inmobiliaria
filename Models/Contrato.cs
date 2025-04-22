using System;

namespace InmobiliariaApp.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }
        public int IdInmueble { get; set; }
        public int IdInquilino { get; set; }
        public int? CreadoPor { get; set; } 
        public int? AnuladoPor { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaInicioAnulacion { get; set; } 
        public DateTime? FechaFinAnulacion { get; set; } 
        public double MontoMensual { get; set; }
        public string? Estado { get; set; }
        // Agregado para la vistas listar
        public string? NyAInquilino { get; set; }
        public string? Direccion { get; set; }
        public string? Portada { get; set; }
        public string? NyAUser { get; set; }
    }
}