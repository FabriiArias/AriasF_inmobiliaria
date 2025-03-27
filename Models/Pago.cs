using System;

namespace InmobiliariaApp.Models
{
    public class Pago
    {
        public int NroPago { get; set; }
        public DateTime FechaPago { get; set; }
        public double Importe { get; set; }
        public int IdContrato { get; set; }
    }
}