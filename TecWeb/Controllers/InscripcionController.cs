using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TecWeb.Core.CustomEntities;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Core.QueryFilters;
using TecWeb.Infrastructure.DTOs;
using Amazon.api.Responses; // tu ApiResponse<T>

namespace TecWeb.Controllers
{
    [Produces("application/json")]
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

        /// <summary>
        /// Lista todas las inscripciones.
        /// </summary>
        /// <returns>Lista paginada de inscripciones como DTO.</returns>
        /// <response code="200">Lista de inscripciones devuelta correctamente</response>
        /// <response code="400">Solicitud inválida</response>
        [HttpGet("listar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<InscripcionDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarInscripciones()
        {
            var result = await _inscripcionService.ListarInscripcionesAsync(null);
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

        /// <summary>
        /// Obtiene una inscripción por su Id.
        /// </summary>
        /// <param name="id">Id de la inscripción a buscar.</param>
        /// <returns>Inscripción como DTO.</returns>
        /// <response code="200">Inscripción encontrada</response>
        /// <response code="404">Inscripción no encontrada</response>
        [HttpGet("buscar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(InscripcionDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ObtenerInscripcion(int id)
        {
            var result = await _inscripcionService.ObtenerInscripcionPorIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Message);

            var dto = _mapper.Map<InscripcionDto>(result.Data);
            return Ok(dto);
        }

        /// <summary>
        /// Crea una nueva inscripción.
        /// </summary>
        /// <param name="insDto">Datos de la inscripción a crear.</param>
        /// <returns>Inscripción creada como DTO.</returns>
        /// <response code="201">Inscripción creada correctamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost("guardar")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(InscripcionDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CrearInscripcion([FromBody] InscripcionDto insDto)
        {
            var entidad = _mapper.Map<Inscripcione>(insDto);
            var result = await _inscripcionService.CrearInscripcionAsync(entidad);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var createdDto = _mapper.Map<InscripcionDto>(result.Data);
            return CreatedAtAction(nameof(ObtenerInscripcion), new { id = createdDto.InscripcionId }, createdDto);
        }

        /// <summary>
        /// Actualiza una inscripción existente.
        /// </summary>
        /// <param name="id">Id de la inscripción a actualizar.</param>
        /// <param name="insDto">Datos actualizados de la inscripción.</param>
        /// <returns>Inscripción actualizada como DTO.</returns>
        /// <response code="200">Inscripción actualizada correctamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPut("actualizar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(InscripcionDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ActualizarInscripcion(int id, [FromBody] InscripcionDto insDto)
        {
            var entidad = _mapper.Map<Inscripcione>(insDto);
            var result = await _inscripcionService.ActualizarInscripcionAsync(id, entidad);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var updatedDto = _mapper.Map<InscripcionDto>(result.Data);
            return Ok(updatedDto);
        }

        /// <summary>
        /// Elimina una inscripción.
        /// </summary>
        /// <param name="id">Id de la inscripción a eliminar.</param>
        /// <response code="204">Inscripción eliminada correctamente</response>
        /// <response code="400">No se puede eliminar la inscripción</response>
        [HttpDelete("eliminar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EliminarInscripcion(int id)
        {
            var result = await _inscripcionService.EliminarInscripcionAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }

        /// <summary>
        /// Lista inscripciones filtradas y paginadas según los parámetros proporcionados.
        /// </summary>
        /// <param name="filters">Filtros para búsqueda y paginación de inscripciones.</param>
        /// <returns>Lista paginada de inscripciones como DTO.</returns>
        /// <response code="200">Lista de inscripciones devuelta correctamente</response>
        /// <response code="400">Filtros inválidos</response>
        [HttpGet("filtrar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<InscripcionDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FiltrarInscripciones([FromQuery] InscripcionQueryFilter filters)
        {
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

        /// <summary>
        /// Endpoint de ejemplo con StatusCode completos para Swagger.
        /// </summary>
        [HttpGet("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<InscripcionDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetInscripcionesDtoMapper([FromQuery] InscripcionQueryFilter filters)
        {
            try
            {
                var result = await _inscripcionService.ListarInscripcionesAsync(filters);

                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                var list = result.Data;
                if (list == null || !list.Any())
                    return NotFound("No se encontraron inscripciones.");

                var dtos = _mapper.Map<IEnumerable<InscripcionDto>>(list);
                return Ok(dtos);
            }
            catch (System.Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
