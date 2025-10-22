using Microsoft.AspNetCore.Mvc;
using TecWeb.Core.DTOs;
using TecWeb.Core.Interfaces;
using System.Threading.Tasks;

namespace TecWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionController : ControllerBase
    {
        private readonly IInscripcionService _inscripcionService;

        public InscripcionController(IInscripcionService inscripcionService)
        {
            _inscripcionService = inscripcionService;
        }

        /
        [HttpGet("listar")]
        public async Task<IActionResult> ListarInscripciones([FromQuery] int? eventoId)
        {
            if (eventoId.HasValue)
            {
                var porEvento = await _inscripcionService.ListarInscripcionesPorEventoAsync(eventoId.Value);
                return porEvento.IsSuccess ? Ok(porEvento.Data) : BadRequest(porEvento.Message);
            }

            
            var all = await _inscripcionService.ListarInscripcionesAsync();
            return all.IsSuccess ? Ok(all.Data) : BadRequest(all.Message);
        }

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> ObtenerInscripcion(int id)
        {
            var result = await _inscripcionService.ObtenerInscripcionPorIdAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.Message);
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> CrearInscripcion([FromBody] InscripcionDto inscripcionDto)
        {
            var result = await _inscripcionService.CrearInscripcionAsync(inscripcionDto);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return CreatedAtAction(nameof(ObtenerInscripcion), new { id = result.Data.InscripcionId }, result.Data);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarInscripcion(int id, [FromBody] InscripcionDto inscripcionDto)
        {
            var result = await _inscripcionService.ActualizarInscripcionAsync(id, inscripcionDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarInscripcion(int id)
        {
            var result = await _inscripcionService.EliminarInscripcionAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }
    }
}
