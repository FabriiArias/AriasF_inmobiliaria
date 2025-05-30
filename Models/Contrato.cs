using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaApp.Models
{
    public class Contrato : IValidatableObject
    {
        public int IdContrato { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un inmueble.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un inmueble válido.")]
        public int IdInmueble { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un inquilino.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un inquilino válido.")]
        public int IdInquilino { get; set; }

        public int? CreadoPor { get; set; }
        public int? AnuladoPor { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public DateTime? FechaInicioAnulacion { get; set; }
        public DateTime? FechaFinAnulacion { get; set; }

        [Required(ErrorMessage = "El monto mensual es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto mensual debe ser mayor a cero.")]
        public double MontoMensual { get; set; }

        public string? Estado { get; set; }

        // Campos para vistas
        public string? NyAInquilino { get; set; }
        public string? Direccion { get; set; }
        public string? Portada { get; set; }
        public string? NyAUser { get; set; }

        // Validación personalizada para fechas
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaInicio >= FechaFin)
            {
                yield return new ValidationResult(
                    "La fecha de inicio debe ser anterior a la fecha de fin.",
                    new[] { nameof(FechaInicio), nameof(FechaFin) });
            }
        }
    }
}
