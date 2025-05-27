using Microsoft.AspNetCore.Mvc;
using AcquisitionAPI.DTOs;
using AcquisitionAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcquisitionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcquisitionsController : ControllerBase
    {
        private readonly IAcquisitionService _acquisitionService;

        public AcquisitionsController(IAcquisitionService acquisitionService)
        {
            _acquisitionService = acquisitionService;
        }

        /// <summary>
        /// Obtiene todas las adquisiciones activas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcquisitionDto>>> GetAll()
        {
            var acquisitions = await _acquisitionService.GetAllAsync();
            return Ok(acquisitions);
        }

        /// <summary>
        /// Crea una nueva adquisición.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AcquisitionDto>> Create([FromBody] AcquisitionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _acquisitionService.CreateAsync(dto, "Creación de adquisición");
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Obtiene una adquisición por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AcquisitionDto>> GetById(int id)
        {
            var dto = await _acquisitionService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        /// <summary>
        /// Actualiza completamente una adquisición.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AcquisitionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _acquisitionService.UpdateAsync(id, dto, "Actualización completa de adquisición");
            return result ? Ok(dto) : NotFound();
        }

        /// <summary>
        /// Actualiza parcialmente una adquisición (por ahora igual a PUT).
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] AcquisitionDto dto)
        {
            var result = await _acquisitionService.PatchAsync(id, dto, "Actualización parcial de adquisición");
            return result ? Ok(dto) : NotFound();
        }

        /// <summary>
        /// Desactiva una adquisición lógicamente (no se elimina físicamente).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _acquisitionService.DeactivateAsync(id, "Desactivación lógica de adquisición");
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Obtiene el historial de versiones de una adquisición.
        /// </summary>
        [HttpGet("{id}/history")]
        public async Task<ActionResult<object>> GetHistory(int id)
        {
            var history = await _acquisitionService.GetHistoryAsync(id);

            return Ok(new
            {
                count = history?.Count ?? 0,
                results = history
            });
        }
    }
}
