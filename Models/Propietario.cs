using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaApp.Models
{
    public class Propietario
    {
        [Required(ErrorMessage = "El DNI del propietario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El DNI debe ser un número positivo.")]
        public int DNIPropietario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El celular es obligatorio.")]
        [Range(2, 9999999999, ErrorMessage = "El celular debe ser un número válido.")]
        public int Celular { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El email no puede superar los 100 caracteres.")]
        public string Email { get; set; } = string.Empty;
    }
}
