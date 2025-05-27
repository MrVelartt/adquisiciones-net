using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcquisitionAPI.Models
{
    public class Acquisition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        [Range(0, double.MaxValue)]
        public decimal Presupuesto { get; set; }

        [Required]
        [MaxLength(255)]
        public string Unidad { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string TipoBienServicio { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Range(0, double.MaxValue)]
        public decimal ValorUnitario { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        [Range(0, double.MaxValue)]
        public decimal ValorTotal { get; set; }

        [Required]
        public DateTime FechaAdquisicion { get; set; }

        [Required]
        [MaxLength(255)]
        public string Proveedor { get; set; } = string.Empty;

        public string? Documentacion { get; set; }

        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public void Validar()
        {
            if (FechaAdquisicion > DateTime.Today)
                throw new ValidationException("La fecha de adquisición no puede ser futura.");

            decimal expectedTotal = Cantidad * ValorUnitario;
            if (ValorTotal != expectedTotal)
                throw new ValidationException($"El valor total debe ser igual a cantidad × valor unitario ({expectedTotal}).");
        }
    }
}
