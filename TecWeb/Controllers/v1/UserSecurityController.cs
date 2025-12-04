using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TecWeb.Core.Entities;
using TecWeb.Core.Enum;
using TecWeb.Core.Interfaces;
using TecWeb.Infrastructure.DTOs;

namespace TecWeb.Controllers.v1
{
    [Authorize(Roles = nameof(RoleType.Administrator))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserSecurityController : ControllerBase
    {
        private readonly IUserSecurityService _userSecurityService;
        private readonly IMapper _mapper;

        public UserSecurityController(IUserSecurityService userSecurityService,
            IMapper mapper)
        {
            _userSecurityService = userSecurityService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserSecurityDto userSecurityDto)
        {
            var userSecurity = _mapper.Map<UserSecurity>(userSecurityDto);
            await _userSecurityService.RegisterUser(userSecurity);

            userSecurityDto = _mapper.Map<UserSecurityDto>(userSecurity);
            return Ok(userSecurityDto);
        }
    }
}