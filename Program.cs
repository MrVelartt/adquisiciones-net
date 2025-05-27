using AcquisitionAPI.Data;
using AcquisitionAPI.Models;
using AcquisitionAPI.Services;
using Audit.EntityFramework;
using Audit.Core;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuración de EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Audit.NET
Audit.Core.Configuration.Setup()
    .UseEntityFramework(ef => ef
        .AuditTypeMapper(type => typeof(AuditLog))
        .AuditEntityAction<AuditLog>((ev, entry, entity) =>
        {
            entity.TableName = entry.EntityType.Name;
            entity.Action = entry.Action;
            entity.PrimaryKey = string.Join(",", entry.PrimaryKey.Select(kv => $"{kv.Key}:{kv.Value}"));

            entity.OldValues = entry.Changes != null
                ? JsonSerializer.Serialize(entry.Changes.ToDictionary(c => c.ColumnName, c => c.OriginalValue))
                : null;

            entity.NewValues = entry.Changes != null
                ? JsonSerializer.Serialize(entry.Changes.ToDictionary(c => c.ColumnName, c => c.NewValue))
                : null;

            entity.ChangedColumns = entry.Changes != null
                ? string.Join(",", entry.Changes.Select(c => c.ColumnName))
                : null;

            entity.User = ev.Environment.UserName;
            entity.Date = DateTime.UtcNow;
            entity.Comment = ev.CustomFields?["comment"]?.ToString();
        }));

// Inyección de dependencias
builder.Services.AddScoped<IAcquisitionService, AcquisitionService>();

// Controladores
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
