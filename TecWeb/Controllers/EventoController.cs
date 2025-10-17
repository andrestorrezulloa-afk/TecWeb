using Microsoft.AspNetCore.Mvc;
using TecWeb.DTOs;
using TecWeb.Services;

namespace TecWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarEventos()
        {
            var result = await _eventoService.ListarEventosAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> ObtenerEvento(int id)
        {
            var result = await _eventoService.ObtenerEventoPorIdAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.Message);
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> CrearEvento([FromBody] EventoDto eventoDto)
        {
            var result = await _eventoService.CrearEventoAsync(eventoDto);
            return result.IsSuccess ? Created("", result.Data) : BadRequest(result.Message);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarEvento(int id, [FromBody] EventoDto eventoDto)
        {
            var result = await _eventoService.ActualizarEventoAsync(id, eventoDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarEvento(int id)
        {
            var result = await _eventoService.EliminarEventoAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }
    }
}