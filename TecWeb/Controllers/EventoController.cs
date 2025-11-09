using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.CustomEntities; // <- Para PagedList y Pagination
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Core.QueryFilters;
using TecWeb.Infrastructure.DTOs;
using Amazon.api.Responses; // <-- tu ApiResponse (asegúrate que exista)

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
            var evento = _mapper.Map<Evento>(eventoDto);

            var result = await _eventoService.CrearEventoAsync(evento);
            if (!result.IsSuccess) return BadRequest(result.Message);

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

        // =============================================
        // FILTRO + PAGINACIÓN
        // =============================================
        [HttpGet("filtrar")]
        public async Task<IActionResult> ListarEventosFiltrados([FromQuery] EventoQueryFilter filters)
        {
            var result = await _eventoService.ListarEventosFiltradosAsync(filters);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var eventosPaged = result.Data; // PagedList<Evento>
            var eventosDto = _mapper.Map<IEnumerable<EventoDto>>(eventosPaged);

            // Mapear metadatos de paginación desde PagedList<Evento>
            var pagination = new Pagination
            {
                TotalCount = eventosPaged.TotalCount,
                PageSize = eventosPaged.PageSize,
                CurrentPage = eventosPaged.CurrentPage,
                TotalPages = eventosPaged.TotalPages,
                HasNextPage = eventosPaged.HasNextPage,
                HasPreviousPage = eventosPaged.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<EventoDto>>(eventosDto)
            {
                Pagination = pagination
            };

            return Ok(response);
        }
    }
}
