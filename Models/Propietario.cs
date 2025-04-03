using System;

namespace InmobiliariaApp.Models
{
    public class Propietario
    {
        public int DNIPropietario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Celular { get; set; }
        public string Email { get; set; } = string.Empty;
        
    }
}