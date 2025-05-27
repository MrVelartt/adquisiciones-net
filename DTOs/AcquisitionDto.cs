using System;
using System.ComponentModel.DataAnnotations;

namespace AcquisitionAPI.DTOs
{
    public class AcquisitionDto
    {
        public int Id { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El presupuesto debe ser mayor o igual a 0.")]
        public decimal Presupuesto { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres.")]
        public string Unidad { get; set; } = string.Empty;

        [Required]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres.")]
        public string TipoBienServicio { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El valor unitario debe ser mayor o igual a 0.")]
        public decimal ValorUnitario { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El valor total debe ser mayor o igual a 0.")]
        public decimal ValorTotal { get; set; }

        [Required(ErrorMessage = "La fecha de adquisición es obligatoria.")]
        public DateTime FechaAdquisicion { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Máximo 255 caracteres.")]
        public string Proveedor { get; set; } = string.Empty;

        public string? Documentacion { get; set; }

        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
