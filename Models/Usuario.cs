using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaApp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El email no puede superar los 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(50, ErrorMessage = "El rol no puede superar los 50 caracteres.")]
        public string Rol { get; set; } = string.Empty;

        public string? Avatar { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El contacto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El contacto no puede superar los 100 caracteres.")]
        public string Contacto { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
