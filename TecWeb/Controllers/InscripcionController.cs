using Microsoft.AspNetCore.Mvc;
using TecWeb.DTOs;
using TecWeb.Services;

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

        [HttpGet("listar")]
        public async Task<IActionResult> ListarInscripciones()
        {
            var result = await _inscripcionService.ListarInscripcionesAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> ObtenerInscripcion(int id)
        {
            var result = await _inscripcionService.ObtenerInscripcionPorIdAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.Message);
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> CrearInscripcion([FromBody] InscripcioneDto inscripcionDto)
        {
            var result = await _inscripcionService.CrearInscripcionAsync(inscripcionDto);
            return result.IsSuccess ? Created("", result.Data) : BadRequest(result.Message);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarInscripcion(int id, [FromBody] InscripcioneDto inscripcionDto)
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