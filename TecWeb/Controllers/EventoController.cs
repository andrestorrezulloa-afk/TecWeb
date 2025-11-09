using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWeb.Core.CustomEntities; // PagedList, Pagination
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Core.QueryFilters;
using TecWeb.Infrastructure.DTOs;
using Amazon.api.Responses; // tu ApiResponse<T>
using System.Net;

namespace TecWeb.Controllers
{
    [Produces("application/json")]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<EventoDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarEventos()
        {
            var result = await _eventoService.ListarEventosAsync();
            if (!result.IsSuccess) return BadRequest(result.Message);

            var eventosDto = _mapper.Map<IEnumerable<EventoDto>>(result.Data);
            return Ok(eventosDto);
        }

        [HttpGet("buscar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EventoDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ObtenerEvento(int id)
        {
            var result = await _eventoService.ObtenerEventoPorIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Message);

            var eventoDto = _mapper.Map<EventoDto>(result.Data);
            return Ok(eventoDto);
        }

        [HttpPost("guardar")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(EventoDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CrearEvento([FromBody] EventoDto eventoDto)
        {
            var evento = _mapper.Map<Evento>(eventoDto);
            var result = await _eventoService.CrearEventoAsync(evento);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var createdDto = _mapper.Map<EventoDto>(result.Data);
            return CreatedAtAction(nameof(ObtenerEvento), new { id = createdDto.EventoId }, createdDto);
        }

        [HttpPut("actualizar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EventoDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ActualizarEvento(int id, [FromBody] EventoDto eventoDto)
        {
            var evento = _mapper.Map<Evento>(eventoDto);
            var result = await _eventoService.ActualizarEventoAsync(id, evento);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var updatedDto = _mapper.Map<EventoDto>(result.Data);
            return Ok(updatedDto);
        }

        [HttpDelete("eliminar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EliminarEvento(int id)
        {
            var result = await _eventoService.EliminarEventoAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }

        [HttpGet("filtrar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<EventoDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarEventosFiltrados([FromQuery] EventoQueryFilter filters)
        {
            var result = await _eventoService.ListarEventosFiltradosAsync(filters);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var eventosPaged = result.Data;
            var eventosDto = _mapper.Map<IEnumerable<EventoDto>>(eventosPaged);

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

        // =====================================
        // Nuevo endpoint con todos los StatusCode
        // =====================================
        [HttpGet("dto/mapper/")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<EventoDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetEventosDtoMapper([FromQuery] EventoQueryFilter filters)
        {
            try
            {
                var result = await _eventoService.ListarEventosFiltradosAsync(filters);
                if (result == null || result.Data == null || !result.Data.Any())
                    return NotFound("No se encontraron eventos.");

                var dtos = _mapper.Map<IEnumerable<EventoDto>>(result.Data);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
