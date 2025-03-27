using System;

namespace InmobiliariaApp.Models
{
    public class Inmueble
    {
        public int IdInmueble { get; set; }
        public int DNIPropietario { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int Ambientes { get; set; }
        public string Coordenadas { get; set; } = string.Empty;
        public double Precio { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Uso { get; set; } = string.Empty;

    }
}