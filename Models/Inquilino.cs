using System;

namespace InmobiliariaApp.Models
{
    public class Inquilino
    {
        public int Id { get; set; }
        public int DNIInquilino { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Celular { get; set; }
    }
}