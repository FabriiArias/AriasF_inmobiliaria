using System;

namespace InmobiliariaApp.Models
{
    public class Contrato
    {
        public int IdContrato { get; set; }
        public int DNIInquilino { get; set; }
        public int IdInmueble { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public double MontoMensual { get; set; }
        public double Multa { get; set; }
        public bool Activo { get; set; }
    }
}