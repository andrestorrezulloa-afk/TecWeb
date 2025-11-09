using Microsoft.AspNetCore.Mvc;
using TecWeb.Infrastructure.DTOs;
using TecWeb.Core.Interfaces;
using TecWeb.Core.Entities;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TecWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var result = await _usuarioService.ListarUsuariosAsync();
            if (!result.IsSuccess) return BadRequest(result.Message);

            var dtos = _mapper.Map<IEnumerable<UsuarioDto>>(result.Data);
            return Ok(dtos);
        }

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            var result = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            if (!result.IsSuccess) return NotFound(result.Message);

            var dto = _mapper.Map<UsuarioDto>(result.Data);
            return Ok(dto);
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var entity = _mapper.Map<Usuario>(usuarioDto);
            var result = await _usuarioService.CrearUsuarioAsync(entity);

            if (!result.IsSuccess) return BadRequest(result.Message);

            var createdDto = _mapper.Map<UsuarioDto>(result.Data);
            return CreatedAtAction(nameof(ObtenerUsuario), new { id = createdDto.UsuarioId }, createdDto);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioDto usuarioDto)
        {
            var entity = _mapper.Map<Usuario>(usuarioDto);
            var result = await _usuarioService.ActualizarUsuarioAsync(id, entity);

            if (!result.IsSuccess) return BadRequest(result.Message);

            var updatedDto = _mapper.Map<UsuarioDto>(result.Data);
            return Ok(updatedDto);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var result = await _usuarioService.EliminarUsuarioAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }
    }
}
