using System;

namespace InmobiliariaApp.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }
        public int IdInmueble { get; set; }
        public int IdInquilino { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double MontoMensual { get; set; }
        public double? Multa { get; set; }
        public string CreadoPor { get; set; } 
        public string? AnuladoPor { get; set; }
        public bool Activo { get; set; }
    }
}