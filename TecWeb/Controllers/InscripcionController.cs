using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.CustomEntities;       // PagedList, Pagination
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Core.QueryFilters;
using TecWeb.Infrastructure.DTOs;
using Amazon.api.Responses;             // tu ApiResponse<T>

namespace TecWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionController : ControllerBase
    {
        private readonly IInscripcionService _inscripcionService;
        private readonly IMapper _mapper;

        public InscripcionController(IInscripcionService inscripcionService, IMapper mapper)
        {
            _inscripcionService = inscripcionService;
            _mapper = mapper;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarInscripciones()
        {
            // Llama al service sin filtros -> por defecto paginado (si tu service soporta null)
            var result = await _inscripcionService.ListarInscripcionesAsync(null);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var paged = result.Data; // PagedList<Inscripcione>
            var dtos = _mapper.Map<IEnumerable<InscripcionDto>>(paged);

            var pagination = new Pagination
            {
                TotalCount = paged.TotalCount,
                PageSize = paged.PageSize,
                CurrentPage = paged.CurrentPage,
                TotalPages = paged.TotalPages,
                HasNextPage = paged.HasNextPage,
                HasPreviousPage = paged.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<InscripcionDto>>(dtos)
            {
                Pagination = pagination
            };

            return Ok(response);
        }

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> ObtenerInscripcion(int id)
        {
            var result = await _inscripcionService.ObtenerInscripcionPorIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Message);

            var dto = _mapper.Map<InscripcionDto>(result.Data);
            return Ok(dto);
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> CrearInscripcion([FromBody] InscripcionDto insDto)
        {
            var entidad = _mapper.Map<Inscripcione>(insDto);
            var result = await _inscripcionService.CrearInscripcionAsync(entidad);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var createdDto = _mapper.Map<InscripcionDto>(result.Data);
            return CreatedAtAction(nameof(ObtenerInscripcion), new { id = createdDto.InscripcionId }, createdDto);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarInscripcion(int id, [FromBody] InscripcionDto insDto)
        {
            var entidad = _mapper.Map<Inscripcione>(insDto);
            var result = await _inscripcionService.ActualizarInscripcionAsync(id, entidad);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var updatedDto = _mapper.Map<InscripcionDto>(result.Data);
            return Ok(updatedDto);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarInscripcion(int id)
        {
            var result = await _inscripcionService.EliminarInscripcionAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }

        // FILTRAR + PAGINACIÓN (recibe query params)
        [HttpGet("filtrar")]
        public async Task<IActionResult> FiltrarInscripciones([FromQuery] InscripcionQueryFilter filters)
        {
            // Asume que ListarInscripcionesAsync retorna ServiceResult<PagedList<Inscripcione>>
            var result = await _inscripcionService.ListarInscripcionesAsync(filters);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var paged = result.Data;
            var dtos = _mapper.Map<IEnumerable<InscripcionDto>>(paged);

            var pagination = new Pagination
            {
                TotalCount = paged.TotalCount,
                PageSize = paged.PageSize,
                CurrentPage = paged.CurrentPage,
                TotalPages = paged.TotalPages,
                HasNextPage = paged.HasNextPage,
                HasPreviousPage = paged.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<InscripcionDto>>(dtos)
            {
                Pagination = pagination
            };

            return Ok(response);
        }
    }
}
