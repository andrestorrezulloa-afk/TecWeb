using System.Collections.Generic;
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

        public async Task<ServiceResult<List<UsuarioDto>>> ListarUsuariosAsync()
        {
            var usuarios = await _repo.ListarAsync();
            return ServiceResult<List<UsuarioDto>>.Success(_mapper.Map<List<UsuarioDto>>(usuarios));
        }

        public async Task<ServiceResult<UsuarioDto>> ObtenerUsuarioPorIdAsync(int id)
        {
            var u = await _repo.ObtenerPorIdAsync(id);
            if (u == null) return ServiceResult<UsuarioDto>.Failure("Usuario no encontrado");
            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(u));
        }

        public async Task<ServiceResult<UsuarioDto>> CrearUsuarioAsync(UsuarioDto dto)
        {
            if (dto == null) return ServiceResult<UsuarioDto>.Failure("Usuario nulo");
            if (await _repo.CorreoExisteAsync(dto.Correo)) return ServiceResult<UsuarioDto>.Failure("Correo ya registrado");

            var entidad = _mapper.Map<Usuario>(dto);
            var creado = await _repo.CrearAsync(entidad);
            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(creado), "Usuario creado");
        }

        public async Task<ServiceResult<UsuarioDto>> ActualizarUsuarioAsync(int id, UsuarioDto dto)
        {
            var u = await _repo.ObtenerPorIdAsync(id);
            if (u == null) return ServiceResult<UsuarioDto>.Failure("Usuario no encontrado");

            _mapper.Map(dto, u);
            await _repo.ActualizarAsync(u);
            return ServiceResult<UsuarioDto>.Success(_mapper.Map<UsuarioDto>(u), "Usuario actualizado");
        }

        public async Task<ServiceResult<bool>> EliminarUsuarioAsync(int id)
        {
            var u = await _repo.ObtenerPorIdAsync(id);
            if (u == null) return ServiceResult<bool>.Failure("Usuario no encontrado");
            await _repo.EliminarAsync(u);
            return ServiceResult<bool>.Success(true, "Usuario eliminado");
        }
    }
}
