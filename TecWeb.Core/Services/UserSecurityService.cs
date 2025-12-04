using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;

namespace TecWeb.Core.Services
{
    public class UserSecurityService : IUserSecurityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserSecurity?> GetLoginByCredentials(UserLogin userLogin)
        {
            return await _unitOfWork.UserSecurityRepository.GetLoginByCredentials(userLogin);
        }

        public async Task RegisterUser(UserSecurity userSecurity)
        {
            await _unitOfWork.UserSecurityRepository.Add(userSecurity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}