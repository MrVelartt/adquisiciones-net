using System;
using System.Text.Json;

namespace AcquisitionAPI.DTOs
{
    public class VersionDto
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Comentario { get; set; } = string.Empty;

        /// <summary>
        /// Datos versionados serializados como JSON.
        /// Puede contener una instancia previa o posterior del objeto.
        /// </summary>
        public JsonElement Datos { get; set; }
    }
}
