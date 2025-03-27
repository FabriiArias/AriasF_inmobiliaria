using System;

namespace InmobiliariaApp.Models
{
    public class Inquilino
    {
        public int DNIInquilino { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
    }
}