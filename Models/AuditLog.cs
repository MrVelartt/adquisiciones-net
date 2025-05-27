using System;

namespace AcquisitionAPI.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string TableName { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string PrimaryKey { get; set; } = string.Empty;

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public string? ChangedColumns { get; set; }

        public string? User { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string? Comment { get; set; }
    }
}
