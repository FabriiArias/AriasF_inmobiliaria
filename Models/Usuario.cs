using System;

namespace InmobiliariaApp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}