using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;
using TecWeb.Core.Exceptions;
using TecWeb.Core.Interfaces;

namespace TecWeb.Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<List<Usuario>>> ListarUsuariosAsync()
        {
            var usuarios = _unitOfWork.UsuarioRepository.GetAll();
            return ServiceResult<List<Usuario>>.Success(new List<Usuario>(usuarios));
        }

        public async Task<ServiceResult<Usuario>> ObtenerUsuarioPorIdAsync(int id)
        {
            var u = await _unitOfWork.UsuarioRepository.GetById(id);
            if (u == null)
                return ServiceResult<Usuario>.Failure("Usuario no encontrado");

            return ServiceResult<Usuario>.Success(u);
        }

        public async Task<ServiceResult<Usuario>> CrearUsuarioAsync(Usuario usuario)
        {
            if (usuario == null)
                return ServiceResult<Usuario>.Failure("Usuario nulo");

            if (await _unitOfWork.UsuarioRepository.CorreoExisteAsync(usuario.Correo))
                return ServiceResult<Usuario>.Failure("Correo ya registrado");

            await _unitOfWork.UsuarioRepository.Add(usuario);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<Usuario>.Success(usuario, "Usuario creado");
        }

        public async Task<ServiceResult<Usuario>> ActualizarUsuarioAsync(int id, Usuario usuario)
        {
            var u = await _unitOfWork.UsuarioRepository.GetById(id);
            if (u == null)
                return ServiceResult<Usuario>.Failure("Usuario no encontrado");

            // Actualizar propiedades
            u.Nombre = usuario.Nombre;
            u.Apellido = usuario.Apellido;
            u.Correo = usuario.Correo;
            u.Telefono = usuario.Telefono;
            u.Rol = usuario.Rol;
            u.FechaRegistro = usuario.FechaRegistro;

            _unitOfWork.UsuarioRepository.Update(u);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<Usuario>.Success(u, "Usuario actualizado");
        }

        public async Task<ServiceResult<bool>> EliminarUsuarioAsync(int id)
        {
            var u = await _unitOfWork.UsuarioRepository.GetById(id);
            if (u == null)
                return ServiceResult<bool>.Failure("Usuario no encontrado");

            _unitOfWork.UsuarioRepository.Delete(u);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<bool>.Success(true, "Usuario eliminado");
        }
    }
}
