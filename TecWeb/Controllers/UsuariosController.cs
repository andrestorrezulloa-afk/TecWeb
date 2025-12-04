using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Infrastructure.DTOs;
using Amazon.api.Responses; // tu ApiResponse<T>

namespace TecWeb.Controllers.v1
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos los usuarios.
        /// </summary>
        /// <returns>Lista de usuarios como DTO.</returns>
        /// <response code="200">Lista de usuarios devuelta correctamente</response>
        /// <response code="400">Solicitud inválida</response>
        [HttpGet("listar")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<UsuarioDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ListarUsuarios()
        {
            var result = await _usuarioService.ListarUsuariosAsync();
            if (!result.IsSuccess) return BadRequest(result.Message);

            var dtos = _mapper.Map<IEnumerable<UsuarioDto>>(result.Data);
            return Ok(dtos);
        }

        /// <summary>
        /// Obtiene un usuario por su Id.
        /// </summary>
        /// <param name="id">Id del usuario a buscar.</param>
        /// <returns>Usuario como DTO.</returns>
        /// <response code="200">Usuario encontrado</response>
        /// <response code="404">Usuario no encontrado</response>
        [HttpGet("buscar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UsuarioDto))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            var result = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Message);

            var dto = _mapper.Map<UsuarioDto>(result.Data);
            return Ok(dto);
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="usuarioDto">Datos del usuario a crear.</param>
        /// <returns>Usuario creado como DTO.</returns>
        /// <response code="201">Usuario creado correctamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost("guardar")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(UsuarioDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var entity = _mapper.Map<Usuario>(usuarioDto);
            var result = await _usuarioService.CrearUsuarioAsync(entity);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var createdDto = _mapper.Map<UsuarioDto>(result.Data);

            // Incluimos la versión en las route values para que CreatedAtAction genere la URL con /api/v1/...
            return CreatedAtAction(
                nameof(ObtenerUsuario),
                new { version = "1.0", id = createdDto.UsuarioId },
                createdDto
            );
        }

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        /// <param name="id">Id del usuario a actualizar.</param>
        /// <param name="usuarioDto">Datos actualizados del usuario.</param>
        /// <returns>Usuario actualizado como DTO.</returns>
        /// <response code="200">Usuario actualizado correctamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPut("actualizar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UsuarioDto))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioDto usuarioDto)
        {
            var entity = _mapper.Map<Usuario>(usuarioDto);
            var result = await _usuarioService.ActualizarUsuarioAsync(id, entity);
            if (!result.IsSuccess) return BadRequest(result.Message);

            var updatedDto = _mapper.Map<UsuarioDto>(result.Data);
            return Ok(updatedDto);
        }

        /// <summary>
        /// Elimina un usuario.
        /// </summary>
        /// <param name="id">Id del usuario a eliminar.</param>
        /// <response code="204">Usuario eliminado correctamente</response>
        /// <response code="400">No se puede eliminar el usuario</response>
        [HttpDelete("eliminar/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var result = await _usuarioService.EliminarUsuarioAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }

        /// <summary>
        /// Endpoint de ejemplo con StatusCode completos para Swagger.
        /// </summary>
        [HttpGet("dto/mapper")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<UsuarioDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUsuariosDtoMapper()
        {
            try
            {
                var result = await _usuarioService.ListarUsuariosAsync();
                if (!result.IsSuccess)
                    return BadRequest(result.Message);

                var list = result.Data;
                if (list == null || !list.Any())
                    return NotFound("No se encontraron usuarios.");

                var dtos = _mapper.Map<IEnumerable<UsuarioDto>>(list);
                return Ok(dtos);
            }
            catch (System.Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
