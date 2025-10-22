using System.Threading.Tasks;
using AutoMapper;
using TecWeb.Core.DTOs;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Core.Services;

namespace TecWeb.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ServiceResult<UsuarioDto>> CrearUsuarioAsync(UsuarioDto usuarioDto)
        {
            if (usuarioDto == null) return ServiceResult<UsuarioDto>.Failure("Usuario nulo");

            if (await _repo.CorreoExisteAsync(usuarioDto.Correo))
                return ServiceResult<UsuarioDto>.Failure("Correo ya registrado");

            var entidad = _mapper.Map<Usuario>(usuarioDto);
            var creado = await _repo.CrearAsync(entidad);
            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(creado), "Usuario creado");
        }

        public async Task<ServiceResult<bool>> EliminarUsuarioAsync(int id)
        {
            var u = await _repo.ObtenerPorIdAsync(id);
            if (u == null) return ServiceResult<bool>.Failure("Usuario no encontrado");
            await _repo.EliminarAsync(u);
            return ServiceResult<bool>.Success(true, "Usuario eliminado");
        }

        public async Task<ServiceResult<UsuarioDto>> ObtenerUsuarioPorIdAsync(int id)
        {
            var u = await _repo.ObtenerPorIdAsync(id);
            if (u == null) return ServiceResult<UsuarioDto>.Failure("Usuario no encontrado");
            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(u));
        }

        public async Task<ServiceResult<UsuarioDto>> ActualizarUsuarioAsync(int id, UsuarioDto usuarioDto)
        {
            var u = await _repo.ObtenerPorIdAsync(id);
            if (u == null) return ServiceResult<UsuarioDto>.Failure("Usuario no encontrado");

            _mapper.Map(usuarioDto, u);
            await _repo.ActualizarAsync(u);
            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(u), "Usuario actualizado");
        }
    }
}
