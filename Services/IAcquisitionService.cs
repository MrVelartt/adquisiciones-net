using AcquisitionAPI.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AcquisitionAPI.Services
{
    public interface IAcquisitionService
    {
        Task<List<AcquisitionDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AcquisitionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AcquisitionDto> CreateAsync(AcquisitionDto dto, string comment, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, AcquisitionDto dto, string comment, CancellationToken cancellationToken = default);
        Task<bool> PatchAsync(int id, AcquisitionDto dto, string comment, CancellationToken cancellationToken = default);
        Task<bool> DeactivateAsync(int id, string comment, CancellationToken cancellationToken = default);
        Task<List<VersionDto>> GetHistoryAsync(int id, CancellationToken cancellationToken = default);
    }
}
