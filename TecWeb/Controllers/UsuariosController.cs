using Microsoft.AspNetCore.Mvc;
using TecWeb.Core.DTOs;
using TecWeb.Core.Interfaces;
using System.Threading.Tasks;

namespace TecWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var result = await _usuarioService.ListarUsuariosAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            var result = await _usuarioService.ObtenerUsuarioPorIdAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.Message);
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var result = await _usuarioService.CrearUsuarioAsync(usuarioDto);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return CreatedAtAction(nameof(ObtenerUsuario), new { id = result.Data.UsuarioId }, result.Data);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioDto usuarioDto)
        {
            var result = await _usuarioService.ActualizarUsuarioAsync(id, usuarioDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var result = await _usuarioService.EliminarUsuarioAsync(id);
            return result.IsSuccess ? NoContent() : BadRequest(result.Message);
        }
    }
}
