using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Core.QueryFilters;
using TecWeb.Infrastructure.DTOs;

namespace TecWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IMapper _mapper;

        public EventoController(IEventoService eventoService, IMapper mapper)
        {
            _eventoService = eventoService;
            _mapper = mapper;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarEventos()
        {
            var result = await _eventoService.ListarEventosAsync();
            if (!result.IsSuccess) return BadRequest(result.Message);

            var eventosDto = _mapper.Map<IEnumerable<EventoDto>>(result.Data);
            return Ok(eventosDto);
        }

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> ObtenerEvento(int id)
        {
            var result = await _eventoService.ObtenerEventoPorIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Message);

            var eventoDto = _mapper.Map<EventoDto>(result.Data);
            return Ok(eventoDto);
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> CrearEvento([FromBody] EventoDto eventoDto)
        {
            // DTO → Entidad
            var evento = _mapper.Map<Evento>(eventoDto);

            var result = await _eventoService.CrearEventoAsync(evento);
            if (!result.IsSuccess) return BadRequest(result.Message);

            // Entidad → DTO
            var createdDto = _mapper.Map<EventoDto>(result.Data);
            return CreatedAtAction(nameof(ObtenerEvento), new { id = createdDto.EventoId }, createdDto);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarEvento(int id, [FromBody] EventoDto eventoDto)
        {
            var evento = _mapper.Map<Evento>(eventoDto);
            var result = await _eventoService.ActualizarEventoAsync(id, evento);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var updatedDto = _mapper.Map<EventoDto>(result.Data);
            return Ok(updatedDto);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarEvento(int id)
        {
            var result = await _eventoService.EliminarEventoAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }

        [HttpGet("filtrar")]
        public async Task<IActionResult> ListarEventosFiltrados([FromQuery] EventoQueryFilter filters)
        {
            var result = await _eventoService.ListarEventosFiltradosAsync(filters);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var eventosDto = _mapper.Map<IEnumerable<EventoDto>>(result.Data);
            return Ok(eventosDto);
        }

    }
}
