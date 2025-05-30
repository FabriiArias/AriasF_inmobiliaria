using System;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaApp.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }

        [Required(ErrorMessage = "El contrato es obligatorio.")]
        public int IdContrato { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime? FechaPago { get; set; }

        [Required(ErrorMessage = "El importe es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor que cero.")]
        public double Importe { get; set; }

        [StringLength(250, ErrorMessage = "El detalle no puede superar los 250 caracteres.")]
        public string? Detalle { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(50, ErrorMessage = "El estado no puede superar los 50 caracteres.")]
        public string? Estado { get; set; }

        [Required]
        public int CreadoPor { get; set; }

        public int? AnuladoPor { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre y Apellido Usuario")]
        public string? NyAUser { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre y Apellido Inquilino")]
        public string? NyAInquilino { get; set; }
    }
}
