using AcquisitionAPI.DTOs;
using AcquisitionAPI.Models;
using AcquisitionAPI.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Audit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcquisitionAPI.Services
{
    public class AcquisitionService : IAcquisitionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AcquisitionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AcquisitionDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var acquisitions = await _context.Acquisitions
                .OrderByDescending(a => a.FechaAdquisicion)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<AcquisitionDto>>(acquisitions);
        }

        public async Task<AcquisitionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var acquisition = await _context.Acquisitions
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            return acquisition == null ? null : _mapper.Map<AcquisitionDto>(acquisition);
        }

        public async Task<AcquisitionDto> CreateAsync(AcquisitionDto dto, string comment, CancellationToken cancellationToken = default)
        {
            var acquisition = _mapper.Map<Acquisition>(dto);
            acquisition.FechaCreacion = DateTime.UtcNow;
            acquisition.FechaActualizacion = DateTime.UtcNow;

            using var scope = AuditScope.Create("Acquisition:Create", () => acquisition, new { comment });

            _context.Acquisitions.Add(acquisition);
            await _context.SaveChangesAsync(cancellationToken);

            scope.SetCustomField("EntityId", acquisition.Id);

            return _mapper.Map<AcquisitionDto>(acquisition);
        }

        public async Task<bool> UpdateAsync(int id, AcquisitionDto dto, string comment, CancellationToken cancellationToken = default)
        {
            var acquisition = await _context.Acquisitions.FindAsync(new object[] { id }, cancellationToken);
            if (acquisition == null) return false;

            _mapper.Map(dto, acquisition);
            acquisition.FechaActualizacion = DateTime.UtcNow;

            using var scope = AuditScope.Create("Acquisition:Update", () => acquisition, new { comment });

            _context.Acquisitions.Update(acquisition);
            await _context.SaveChangesAsync(cancellationToken);

            scope.SetCustomField("EntityId", id);

            return true;
        }

        public async Task<bool> PatchAsync(int id, AcquisitionDto dto, string comment, CancellationToken cancellationToken = default)
        {
            var acquisition = await _context.Acquisitions.FindAsync(new object[] { id }, cancellationToken);
            if (acquisition == null) return false;

            // Actualización condicional campo por campo
            if (dto.FechaAdquisicion != default)
                acquisition.FechaAdquisicion = dto.FechaAdquisicion;

            if (!string.IsNullOrEmpty(dto.Unidad))
                acquisition.Unidad = dto.Unidad;

            if (!string.IsNullOrEmpty(dto.TipoBienServicio))
                acquisition.TipoBienServicio = dto.TipoBienServicio;

            if (!string.IsNullOrEmpty(dto.Proveedor))
                acquisition.Proveedor = dto.Proveedor;

            if (dto.Cantidad > 0)
                acquisition.Cantidad = dto.Cantidad;

            if (dto.ValorUnitario >= 0)
                acquisition.ValorUnitario = dto.ValorUnitario;

            if (dto.ValorTotal >= 0)
                acquisition.ValorTotal = dto.ValorTotal;

            if (dto.Presupuesto >= 0)
                acquisition.Presupuesto = dto.Presupuesto;

            if (!string.IsNullOrEmpty(dto.Documentacion))
                acquisition.Documentacion = dto.Documentacion;

            acquisition.FechaActualizacion = DateTime.UtcNow;

            using var scope = AuditScope.Create("Acquisition:Patch", () => acquisition, new { comment });

            _context.Acquisitions.Update(acquisition);
            await _context.SaveChangesAsync(cancellationToken);

            scope.SetCustomField("EntityId", id);

            return true;
        }

        public async Task<bool> DeactivateAsync(int id, string comment, CancellationToken cancellationToken = default)
        {
            var acquisition = await _context.Acquisitions.FindAsync(new object[] { id }, cancellationToken);
            if (acquisition == null) return false;

            acquisition.Activa = false;
            acquisition.FechaActualizacion = DateTime.UtcNow;

            using var scope = AuditScope.Create("Acquisition:Deactivate", () => acquisition, new { comment });

            _context.Acquisitions.Update(acquisition);
            await _context.SaveChangesAsync(cancellationToken);

            scope.SetCustomField("EntityId", id);

            return true;
        }

        public async Task<List<VersionDto>> GetHistoryAsync(int id, CancellationToken cancellationToken = default)
        {
            var logs = await _context.AuditLogs
                .Where(log => log.TableName == nameof(Acquisition) && log.PrimaryKey.Contains($"Id:{id}"))
                .OrderByDescending(log => log.Date)
                .ToListAsync(cancellationToken);

            var versiones = new List<VersionDto>();

            foreach (var log in logs)
            {
                JsonElement datosJson;

                if (!string.IsNullOrWhiteSpace(log.NewValues))
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(log.NewValues);
                        datosJson = doc.RootElement.Clone();
                    }
                    catch
                    {
                        datosJson = JsonDocument.Parse("{}").RootElement;
                    }
                }
                else
                {
                    datosJson = JsonDocument.Parse("{}").RootElement;
                }

                versiones.Add(new VersionDto
                {
                    Id = log.Id,
                    Fecha = log.Date,
                    Usuario = log.User ?? string.Empty,
                    Comentario = log.Comment ?? string.Empty,
                    Datos = datosJson
                });
            }

            return versiones;
        }
    }
}
