using Microsoft.EntityFrameworkCore;
using AcquisitionAPI.Models;

namespace AcquisitionAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Acquisition> Acquisitions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}
