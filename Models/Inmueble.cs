using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaApp.Models
{
    public class Inmueble : IValidatableObject
    {
        public int IdInmueble { get; set; }

        [Required(ErrorMessage = "El DNI del propietario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El DNI del propietario debe ser un número positivo.")]
        public int DNIPropietario { get; set; }

        [Required(ErrorMessage = "El tipo de inmueble es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo no puede superar los 50 caracteres.")]
        public string Tipo { get; set; } = "";

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres.")]
        public string Direccion { get; set; } = "";

        [Required(ErrorMessage = "La cantidad de ambientes es obligatoria.")]
        [Range(1, 20, ErrorMessage = "La cantidad de ambientes debe estar entre 1 y 20.")]
        public int Ambientes { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(20, ErrorMessage = "El estado no puede superar los 20 caracteres.")]
        public string Estado { get; set; } = "";

        [Required(ErrorMessage = "El uso es obligatorio.")]
        [StringLength(50, ErrorMessage = "El uso no puede superar los 50 caracteres.")]
        public string Uso { get; set; } = "";

        [Required(ErrorMessage = "La longitud es obligatoria.")]
        [MinLength(1, ErrorMessage = "La longitud debe tener al menos 1 carácter.")]
        [StringLength(50, ErrorMessage = "La longitud no puede superar los 50 caracteres.")]
        public string Longitud { get; set; } = "";

        [Required(ErrorMessage = "La latitud es obligatoria.")]
        [MinLength(1, ErrorMessage = "La latitud debe tener al menos 1 carácter.")]
        [StringLength(50, ErrorMessage = "La latitud no puede superar los 50 caracteres.")]
        public string Latitud { get; set; } = "";

        public string? Portada { get; set; } = null;

        // Campos especiales para la consulta con datos del propietario
        public string NombrePropietario { get; set; } = "";
        public string ApellidoPropietario { get; set; } = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }


    }
}
