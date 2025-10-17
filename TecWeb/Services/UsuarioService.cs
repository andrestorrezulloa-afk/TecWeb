using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TecWeb.DTOs;
using TecWeb.Models;

namespace TecWeb.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly GestionCulturalContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(GestionCulturalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UsuarioDto>> CrearUsuarioAsync(UsuarioDto usuarioDto)
        {
            if (usuarioDto == null)
                return ServiceResult<UsuarioDto>.Failure("Usuario nulo");

            var existeCorreo = await _context.Usuarios.AnyAsync(u => u.Correo == usuarioDto.Correo);
            if (existeCorreo)
                return ServiceResult<UsuarioDto>.Failure("El correo ya existe");

            var usuario = _mapper.Map<Usuario>(usuarioDto);
            usuario.FechaRegistro = DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(usuario), "Usuario creado");
        }

        public async Task<ServiceResult<UsuarioDto>> ActualizarUsuarioAsync(int id, UsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return ServiceResult<UsuarioDto>.Failure("Usuario no encontrado");

            _mapper.Map(usuarioDto, usuario);
            await _context.SaveChangesAsync();

            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(usuario), "Usuario actualizado");
        }

        public async Task<ServiceResult<bool>> EliminarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return ServiceResult<bool>.Failure("Usuario no encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true, "Usuario eliminado");
        }

        public async Task<ServiceResult<List<UsuarioDto>>> ListarUsuariosAsync()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return ServiceResult<List<UsuarioDto>>.Success(_mapper.Map<List<UsuarioDto>>(usuarios));
        }

        public async Task<ServiceResult<UsuarioDto>> ObtenerUsuarioPorIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return ServiceResult<UsuarioDto>.Failure("Usuario no encontrado");

            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(usuario));
        }
    }
}