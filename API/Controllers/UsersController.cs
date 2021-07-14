using System.Linq;
using System.Threading.Tasks;
using API.Common.Enums;
using API.Common.ExtensionMethods;
using API.Common.Interfaces;
using API.Common.Models;
using API.Common.Services;
using API.Configuration;
using API.Controllers._Common;
using API.Controllers.Models.Users.Request;
using API.Data;
using API.Models.Dtos;
using API.Models.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseJson<string>))]
    [Route("api/[controller]")]
    public class UsersController : CoreBaseControler
    {
        private readonly ILogger<AccountController> _logger;
        private readonly APIContext _context;
        private readonly AppSettings.CoreLogicSettingModel _coreLogicSetting;
        private readonly IMapper _mapper;
        private readonly GenrateCodeSerivce _genCodeSerivce;
        private readonly NotificationService _notificationService;
        private readonly HashingService _hashingService;
        private readonly IUserClaimService _userClaimService;
        public UsersController(IConfiguration configuration,
            ILogger<AccountController> logger,
            APIContext context,
            IMapper mapper,
            GenrateCodeSerivce genCodeSerivce,
            NotificationService notificationService,
            HashingService hashingService,
            IUserClaimService userClaimService
            )
        {
            _coreLogicSetting = configuration.Get<AppSettings>().CoreLogicSettings;
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _genCodeSerivce = genCodeSerivce;
            _notificationService = notificationService;
            _hashingService = hashingService;
            _userClaimService = userClaimService;
        }
        [HttpGet]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<Pagination.Response<UserDto>>))]
        public async Task<IActionResult> GetAll([FromQuery] Pagination.Request paging)
        {
            var userQuery = _context.UserEntity.Where(x => !x.IsDeleted)
                .Include(x => x.CompanyMappings)
                .ThenInclude(x => x.Company);

            var userDtos = await userQuery.PagingAsync(paging, x => _mapper.Map<UserDto>(x));
            return OKResponse(userDtos);
        }

        [HttpGet]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<UserDto>))]
        [Route("{id}")]
        public async Task<IActionResult> GetDetail([FromRoute] string id)
        {
            var user = await _context.UserEntity
                .Include(x => x.CompanyMappings)
                .ThenInclude(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id.ToUpper() && !x.IsDeleted);
            if (user == null)
            {
                return BadRequestResponse("User not found");
            }
            var userDto = _mapper.Map<UserDto>(user);
            return OKResponse(userDto);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<UserDto>))]
        [Route("me")]
        public async Task<IActionResult> GetMyInfo()
        {
            //var id = GetUserId();
            var id = _userClaimService.GetCurrentUserId();

            var user = await _context.UserEntity
                .Include(x => x.CompanyMappings)
                .ThenInclude(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (user == null)
            {
                return BadRequestResponse("User not found");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return OKResponse(userDto);
        }

        [HttpPost]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<UserDto>))]
        public async Task<IActionResult> PostCreate([FromBody] UserCreateVm model)
        {
            var isExisted = _context.UserEntity.Any(x => x.Email == model.Email.ToLower());

            if (isExisted)
            {
                return BadRequestResponse("User was existed");
            }
            var user = _mapper.Map<UserEntity>(model);

            // check Company Valid
            var companyIds = user.CompanyMappings.Select(x => x.CompanyId);

            var isCompanyInValid = companyIds.Any(cid => !_context.CompanyEntity.Any(f => f.Id == cid));
            if (isCompanyInValid)
            {
                return BadRequest("One or more companies not found");
            }

            user.Id = FuncHelppers.GenerateGUID();
            user.Password = _hashingService.HashPassword(model.Password);
            //If user create not email confirm send mail to verify
            if (!user.IsEmailConfirm)
            {
                var code = _genCodeSerivce.GenarateVerifyCode(VerifyTypeEnum.VerifyEmail);
                user.EmailVerifyCode = code;
            }

            _context.Add(user);

            await _context.SaveChangesAsync();

            if (!user.IsEmailConfirm)
            {
                await _notificationService.SendVerifyEmail(user.Email, user.EmailVerifyCode);
            }

            var userDto = _mapper.Map<UserDto>(user);
            return OKResponse(userDto, "Create user success");
        }

        [HttpPut]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<UserDto>))]
        public async Task<IActionResult> PutUpdate([FromRoute] string id, [FromBody] UserUpdateVm model)
        {
            var user = await _context.UserEntity
                .Include(x => x.CompanyMappings)
                .ThenInclude(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id.ToUpper() && !x.IsDeleted);

            if (user == null)
            {
                return BadRequestResponse("User not found");
            }
            //Check email if change email
            if (user.Email != model.Email)
            {
                var isEmailExisted = _context.UserEntity.Any(x => x.Email == model.Email.ToLower());
                if (isEmailExisted)
                {
                    return BadRequestResponse("Email is existed");
                }
            }

            user.CompanyMappings.Clear();
            //var userUpdate = _mapper.Map<UserEntity>(model);
            user = _mapper.Map(model, user);
            // check Company Valid
            var companyIds = user.CompanyMappings.Select(x => x.CompanyId);

            var isCompanyInValid = companyIds.Any(cid => !_context.CompanyEntity.Any(f => f.Id == cid));
            if (isCompanyInValid)
            {
                return BadRequest("One or more companies not found");
            }

            //If user create not email confirm send mail to verify
            if (!user.IsEmailConfirm)
            {
                var code = _genCodeSerivce.GenarateVerifyCode(VerifyTypeEnum.VerifyEmail);
                user.EmailVerifyCode = code;
            }
            //update password
            if(model.Password != null)
            {
                user.Password = _hashingService.HashPassword(model.Password);
            }
            
            _context.Update(user);

            await _context.SaveChangesAsync();

            if (!user.IsEmailConfirm)
            {
                await _notificationService.SendVerifyEmail(user.Email, user.EmailVerifyCode);
            }

            var userDto = _mapper.Map<UserDto>(user);
            return OKResponse(userDto, "Update user success");
        }

        [HttpDelete]
        [Authorize(Roles = Roles.GlobalAdmin)]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJson<UserDto>))]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var user = await _context.UserEntity
              .FirstOrDefaultAsync(x => x.Id == id.ToUpper() && !x.IsDeleted);

            if (user == null)
            {
                return BadRequestResponse("User not found");
            }
            user.IsDeleted = true;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return BadRequestResponse("Delete user successful");
        }
    }
}
